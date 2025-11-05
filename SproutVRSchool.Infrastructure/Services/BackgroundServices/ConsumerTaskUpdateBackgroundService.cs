using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.BackgroundServices;

public class ConsumerTaskUpdateBackgroundService : BackgroundService
{
    // ===============================
    // === Fields
    // ===============================

    private readonly ILogger<ConsumerTaskUpdateBackgroundService> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _activeVrLearningSessionsTasks;

    // ===============================
    // === Constructors
    // ===============================

    public ConsumerTaskUpdateBackgroundService(
        ILogger<ConsumerTaskUpdateBackgroundService> logger,
        IDateTimeProvider dateTimeProvider,
        IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _dateTimeProvider = dateTimeProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _activeVrLearningSessionsTasks = new ConcurrentDictionary<string, CancellationTokenSource>();
    }

    // ===============================
    // === Methods
    // ===============================

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConsumerTaskUpdateBackgroundService is starting.");
        IDatabase db = _redis.GetDatabase();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // 1. Get the list of all active vr_learning_session_id from the group of streams
                string?[] activeSessionIds = (await db.SetMembersAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE)).ToStringArray();

                // 2. Assign each Worker Thread to handle individual VrLearningSession's Stream, avoid blocking main thread
                foreach (string sessionId in activeSessionIds)
                {
                    // add the cancellation token for later dispose this running worker thread
                    if (_activeVrLearningSessionsTasks.TryAdd(sessionId!, CancellationTokenSource.CreateLinkedTokenSource(stoppingToken)))
                    {
                        CancellationTokenSource taskCts = _activeVrLearningSessionsTasks[sessionId!];
                        _ = Task.Run(() => ProcessStreamForSessionAsync(sessionId!, taskCts.Token), taskCts.Token);
                    }
                }

                // 3. Stop Worker Thread for VrLearningSession that are no longer active
                var stoppedVrLearningSessions = _activeVrLearningSessionsTasks.Keys.Except(activeSessionIds).ToList();
                foreach (string stoppedId in stoppedVrLearningSessions)
                {
                    if (_activeVrLearningSessionsTasks.TryRemove(stoppedId!, out CancellationTokenSource? taskCts))
                    {
                        // Cancel the CancellationToken to gracefully stop the ProcessStreamForSessionAsync loop.
                        await taskCts.CancelAsync();
                        taskCts.Dispose();
                    }
                }

                // Scanning for every 5s
                await Task.Delay(AppCts.Redis.ACTIVE_VR_LEARNING_SESSIONS_SCAN_INTERVAL_IN_MILSECONDS, stoppingToken);
            }
            catch (Exception ex)
            {
                // UNDONE: Add proper exception handling and logging
                _logger.LogError(ex, "Error in ConsumerTaskUpdateBackgroundService ExecuteAsync loop.");
                await Task.Delay(AppCts.Redis.ACTIVE_VR_LEARNING_SESSIONS_SCAN_INTERVAL_IN_MILSECONDS, stoppingToken);
            }
        }
    }

    /// <summary>
    /// This is the task that 1 worker thread is assigned at to handle resolving event stream
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task ProcessStreamForSessionAsync(string sessionId, CancellationToken cancellationToken)
    {
        try
        {
            IDatabase db = _redis.GetDatabase();
            string streamKey = $"{AppCts.Redis.NAMESPACE_STREAM_EVENT_VR_LEARNING_SESSIONS}:{sessionId}";
            string groupName = "session-processors";
            string consumerName = $"processor-{Guid.NewGuid()}";

            // must try catch here, because if the group already exists, it will throw exception for duplicate userGroup
            try
            {
                await db.StreamCreateConsumerGroupAsync(streamKey, groupName, "0-0", createStream: true);
            }
            catch (RedisServerException ex) when (ex.Message.Contains("already exists"))
            {
                // If the group already exists, that's fine. We just log it and continue.
                _logger.LogInformation(ex, "Consumer group '{GroupName}' already exists for stream '{StreamKey}'.", groupName, streamKey);
            }

            // This is the main consumer loop for this specific session.
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 1. Read a batch of up to 10 new messages from the stream for our group.
                    //    The '>' means "messages that no other consumer in the group has seen yet".
                    //    It will wait (block) for up to 5 seconds if there are no new messages.
                    StreamEntry[] messages = await db.StreamReadGroupAsync(streamKey, groupName, consumerName, ">", count: 10);

                    // Wait a second if there are no messages
                    if (!messages.Any())
                    {
                        continue;
                    }

                    // 2. Process each message in the batch.
                    foreach (StreamEntry message in messages)
                    {
                        Dictionary<string, string> messageDict = message.Values.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());

                        // 3. Check the event type and delegate to the appropriate handler.
                        if (messageDict.TryGetValue("EventType", out string? eventType) && eventType == "TaskUpdate")
                        {
                            bool success = await HandleTaskUpdateEventAsync(db, sessionId, messageDict);

                            // acknowledge it so that it's gonna remove from the stream
                            if (success)
                            {
                                await db.StreamAcknowledgeAsync(streamKey, groupName, message.Id);
                            }
                        }
                        else
                        {
                            // If we don't recognize the event, we still acknowledge it so it doesn't block the stream.
                            await db.StreamAcknowledgeAsync(streamKey, groupName, message.Id);
                        }
                    }
                }
                catch (Exception)
                {
                    await Task.Delay(5000, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CRITICAL ERROR in ProcessStreamForSessionAsync for session {SessionId}. The task has crashed.", sessionId);
        }

        _logger.LogInformation("Chef: Stopping processor task for session: {SessionId}", sessionId);
    }

    private async Task<bool> HandleTaskUpdateEventAsync(IDatabase db, string sessionId, Dictionary<string, string> messageDict)
    {
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{sessionId}";
        string taskPath = $"$.Devices['{messageDict["VRDeviceSerialNumber"]}'].Tasks['{messageDict["VRTaskId"]}']";

        // true/false in the JSON Document for readiablilty
        bool isCorrect = messageDict["IsCorrect"].ToBoolean();
        bool isCompleted = messageDict["IsCompleted"].ToBoolean();

        ITransaction transaction = db.CreateTransaction();
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.Status", JsonSerializer.Serialize(ModelTaskProgressStatus.Completed, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.IsCorrect", JsonSerializer.Serialize(isCorrect));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.IsCompleted", JsonSerializer.Serialize(isCompleted));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.CompletionTimeAtUtc", JsonSerializer.Serialize(_dateTimeProvider.UtcDateTimeNow));

        return await transaction.ExecuteAsync();
    }
}
