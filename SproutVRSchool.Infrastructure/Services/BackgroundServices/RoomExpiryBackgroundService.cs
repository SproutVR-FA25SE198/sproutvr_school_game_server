using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
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
    private readonly IDateTimeProvider _dateTimeProvider;

    // ===============================
    // === Constructors
    // ===============================

    public RoomExpiryBackgroundService(
        ILogger<RoomExpiryBackgroundService> logger,
        IServiceProvider serviceProvider,
        IDateTimeProvider dateTimeProvider,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
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

            // 2. Iterate over all member in the active lists
            using IServiceScope scope = _serviceProvider.CreateScope();
            RedisValue[] activeSessionIds = await _database.SetMembersAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE);

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
                    ITransaction transaction = _database.CreateTransaction();
                    _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Completed, _jsonOptions));
                    _ = transaction.SetMoveAsync(
                        AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE,
                        AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_INACTIVE,
                        vrLearningSessionId
                    );
                    await transaction.ExecuteAsync();
                }
            }
        }
    }
}
