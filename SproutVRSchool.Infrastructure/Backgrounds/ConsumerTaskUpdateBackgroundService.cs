using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Backgrounds;

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

        // 1. Active Room discovery to scan all active VR lessions
        // - Each vr learning session is assigned to 1 worker thread for consuming task update

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                IDatabase db = _redis.GetDatabase();
                string?[] activeSessionIds = (await db.SetMembersAsync(AppCts.Redis.NAMESPACE_ACTIVE_LEARNING_SESSIONS)).ToStringArray();

                // Provice worker thread for each active session
                foreach (string sessionId in activeSessionIds)
                {
                    // add the cancellation token for later dispose this running worker thread
                    if (_activeVrLearningSessionsTasks.TryAdd(sessionId!, CancellationTokenSource.CreateLinkedTokenSource(stoppingToken)))
                    {
                        CancellationTokenSource taskCts = _activeVrLearningSessionsTasks[sessionId!];
                        _ = Task.Run(() => ProcessStreamForSessionAsync(sessionId!, taskCts.Token), taskCts.Token);
                    }
                }

                // Cleanup worker threads for stopped sessions
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
                await Task.Delay(10000, stoppingToken);
            }
        }
    }

    private async Task ProcessStreamForSessionAsync(string sessionId, CancellationToken cancellationToken)
    {
        IDatabase db = _redis.GetDatabase();
        string streamKey = $"{AppCts.Redis.NAMESPACE_STREAM_EVENT_VR_LEARNING_SESSION}:{sessionId}";
        string groupName = "session-processors";
        string consumerName = $"processor-{Guid.NewGuid()}";

        await db.StreamCreateConsumerGroupAsync(streamKey, groupName, "0-0", createStream: true);

        // This is the main consumer loop for this specific session.
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // 1. Read a batch of up to 10 new messages from the stream for our group.
                //    The '>' means "messages that no other consumer in the group has seen yet".
                //    It will wait (block) for up to 5 seconds if there are no new messages.
                StreamEntry[] messages = await db.StreamReadGroupAsync(streamKey, groupName, consumerName, ">", count: 10);

                // No new messages, the loop will continue and check again.
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

                        if (success)
                        {
                            // 4. IMPORTANT: Acknowledge the message so Redis knows it's fully processed.
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
                // By not acknowledging, a failed message can be picked up again by another consumer.
                await Task.Delay(5000, cancellationToken);
            }
        }
    }

    private async Task<bool> HandleTaskUpdateEventAsync(IDatabase db, string sessionId, Dictionary<string, string> messageDict)
    {
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSION}:{sessionId}";
        string taskPath = $"$.Devices['{messageDict["VrDeviceSerialNumber"]}'].Tasks['{messageDict["VrTaskId"]}']";

        ITransaction transaction = db.CreateTransaction();
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.Status", JsonSerializer.Serialize(ModelTaskProgressStatus.Completed, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.IsCorrect", messageDict["isCorrect"]);
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.IsCompleted", messageDict["isCompleted"]);
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{taskPath}.CompletionTimeAtUtc", JsonSerializer.Serialize(_dateTimeProvider.UtcDateTimeNow));

        return await transaction.ExecuteAsync();
    }
}
