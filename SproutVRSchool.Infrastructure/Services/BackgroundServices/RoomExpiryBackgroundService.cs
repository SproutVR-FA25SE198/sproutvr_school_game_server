using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NRedisStack.RedisStackCommands;
using Polly;
using Polly.Registry;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.RoomServices.Publishers;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.BackgroundServices;

/// <summary>
/// Run and checking endtime date for every 5s and move from active list to inactive list
/// </summary>
public class RoomExpiryBackgroundService : BackgroundService
{
    // ===============================
    // === Fields
    // ===============================

    private readonly ILogger<RoomExpiryBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    // ===============================
    // === Constructors
    // ===============================

    public RoomExpiryBackgroundService(
        ILogger<RoomExpiryBackgroundService> logger,
        IServiceProvider serviceProvider,
        ResiliencePipelineProvider<string> resiliencePipelineProvider,
        IDateTimeProvider dateTimeProvider,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _resiliencePipelineProvider = resiliencePipelineProvider;
        _serviceProvider = serviceProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _database = connectionMultiplexer.GetDatabase();
    }

    // ===============================
    // === Methods
    // ===============================

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RoomExpiryBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // 1. Check for every 5s
            await Task.Delay(AppCts.Redis.INACTIVE_VR_LEARNING_SESSIONS_SCAN_INTERVAL_IN_MILSECONDS, stoppingToken);

            using IServiceScope scope = _serviceProvider.CreateScope();
            RedisValue[] activeSessionIds = await _database.SetMembersAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE);

            // 2. Iterate over all member in the active lists
            foreach (RedisValue vrLearningSessionId in activeSessionIds)
            {
                string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSessionId}";
                RedisResult sessionJson = await _database.JSON().GetAsync(sessionKey);

                // If the object is null then just continue processing next active id
                if (sessionJson.IsNull)
                {
                    continue;
                }

                // 2. Deserialize the full object
                ModelVRLearningSession? vrLearningSession = JsonSerializer.Deserialize<ModelVRLearningSession>(sessionJson.ToString()!, _jsonOptions);

                if (vrLearningSession == null)
                {
                    continue;
                }

                // 3. Check the endtime at utc
                if (vrLearningSession.Status == ModelVRLearningSessionStatus.Active &&
                    vrLearningSession.EndTimeAtUtc.HasValue &&
                    vrLearningSession.EndTimeAtUtc.Value <= _dateTimeProvider.UtcDateTimeNow)
                {
                    _logger.LogInformation("Session {VrLearningSessionId} has expired. Moving to inactive queue.", vrLearningSessionId);

                    // 4. Set the status to Completed and move to the inactive list
                    ResiliencePipeline<bool> retryPipeline = _resiliencePipelineProvider.GetPipeline<bool>(AppCts.RetryKeys.REDIS_TRANSACTION_KEY);

                    bool isSuccess = await retryPipeline.ExecuteAsync<bool>(async (cancellationToken) =>
                    {
                        ITransaction transaction = _database.CreateTransaction();
                        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Completed, _jsonOptions));
                        _ = transaction.SetMoveAsync(
                            AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE,
                            AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_INACTIVE,
                            vrLearningSessionId
                        );
                        return await transaction.ExecuteAsync();
                    }, stoppingToken);

                    // 5. Is success, then publishing events to Desktop and VRDevices
                    if (isSuccess)
                    {
                        IRoomPublishingService _serverPublishingService = scope.ServiceProvider.GetRequiredService<IRoomPublishingService>();

                        // 5.1. Publish to VR Devices ENDSIGNAL event on VR Device Channel
                        await _serverPublishingService.PublishEndSessionAsync(
                            vrLearningSessionId!,
                            "The teacher has cancelled the session.");

                        // 5.2. Publish to Desktop App Room State ROOMENDED event on Desktop Channel
                        await _serverPublishingService.PublishRoomEndedAsync(
                            vrLearningSessionId!,
                            new RoomEndedDto()
                        );
                    }
                }
            }
        }
    }
}
