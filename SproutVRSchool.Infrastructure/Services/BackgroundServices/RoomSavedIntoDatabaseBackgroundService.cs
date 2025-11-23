using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Models.VRLearningSession;
using SproutVRSchool.Infrastructure.Services.RoomServices;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.BackgroundServices;

public class RoomSavedIntoDatabaseBackgroundService : BackgroundService
{
    // =============================
    // === Constructors
    // =============================

    private readonly ILogger<RoomSavedIntoDatabaseBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;

    // =============================
    // === Fields
    // =============================

    public RoomSavedIntoDatabaseBackgroundService(
        ILogger<RoomSavedIntoDatabaseBackgroundService> logger,
        IServiceProvider serviceProvider,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _database = connectionMultiplexer.GetDatabase();
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
    }

    // =============================
    // === Methods
    // =============================

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // 1. Scanning on the inactive list for every 5s
            await Task.Delay(AppCts.Redis.SAVE_ROOM_INTO_DB_SCAN_INTERVAL_IN_MILSECONDS, stoppingToken);

            // 2. Atomatically popout the id from the inactive vr learning session
            RedisValue vrLearningSessionId = await _database.SetPopAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_INACTIVE_IDS);

            if (vrLearningSessionId.IsNullOrEmpty)
            {
                continue;
            }

            // 3. Get the vr learning session object
            string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_STATE}:{vrLearningSessionId}";
            try
            {
                RedisResult sessionJson = await _database.JSON().GetAsync(sessionKey);

                if (sessionJson.IsNull)
                {
                    _logger.LogWarning("Session {SessionId} was in cleanup list but its JSON document was missing.", vrLearningSessionId);
                    continue;
                }

                ModelVRLearningSession? sessionData = JsonSerializer.Deserialize<ModelVRLearningSession>(sessionJson.ToString(), _jsonOptions);

                // If those fields are null, process the next room
                if (sessionData == null || sessionData.RoomCode == null || sessionData.VRLearningSessionId == null)
                {
                    continue;
                }

                // 4. Save the final session data into the PostgreSQL database
                await SaveRoomInToDatabaseAsync(sessionData, stoppingToken);

                // 5. Cleanup functions
                await CleanupRoomAfterSavingToDatabaseAsync(sessionData.RoomCode, sessionData.VRLearningSessionId);
                _logger.LogInformation("Successfully saved and removed session {SessionId} from Redis.", vrLearningSessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clean up session {SessionId}. This session may need manual processing.", vrLearningSessionId);
            }
        }
    }

    /// <summary>
    /// Cleanup all of the room's metadata
    /// </summary>
    /// <param name="roomCode"></param>
    /// <param name="vrLearningSessionId"></param>
    /// <returns></returns>
    private async Task CleanupRoomAfterSavingToDatabaseAsync(string roomCode, string vrLearningSessionId)
    {
        string roomCodeKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ROOM_CODES}:{roomCode}";
        string activeSet = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE_IDS}";
        string inactiveSet = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_INACTIVE_IDS}";
        string streamKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_STREAMS_TASK_UPDATED_EVENTS}:{vrLearningSessionId}";
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_STATE}:{vrLearningSessionId}";

        ITransaction trans = _database.CreateTransaction();
        _ = trans.KeyDeleteAsync(roomCodeKey);
        _ = trans.SetRemoveAsync(activeSet, vrLearningSessionId);
        _ = trans.SetRemoveAsync(inactiveSet, vrLearningSessionId);
        _ = trans.KeyDeleteAsync(streamKey);
        _ = trans.KeyDeleteAsync(sessionKey);

        if (await trans.ExecuteAsync())
        {
            _logger.LogInformation("CleanUpRoomStateAsync: Clean up all room's information");
        }
        else
        {
            _logger.LogCritical("CleanUpRoomStateAsync: Room's information cannot be cleaned. Please try again");
        }
    }

    /// <summary>
    /// A method to store the inactive room into 3 history table: 
    /// </summary>
    /// <param name="uow"></param>
    /// <param name="modelVRLearningSession"></param>
    /// <param name="dateTimeProvider"></param>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task SaveRoomInToDatabaseAsync(ModelVRLearningSession? modelVRLearningSession, CancellationToken stoppingToken)
    {
        // 1. If no data, then dont do anything
        if (modelVRLearningSession == null)
        {
            return;
        }

        // 2. Get all essential services
        using IServiceScope scope = _serviceProvider.CreateScope();
        IUnitOfWork uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        IDateTimeProvider dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        DateTimeOffset currentDateUtcNow = dateTimeProvider.UtcDateTimeNow;

        try
        {
            // 3. Save into the VRLearningSession table
            var newVrLearningSession = VRLearningSession.Create(modelVRLearningSession, currentDateUtcNow);
            uow.Repository<VRLearningSession>().Add(newVrLearningSession);

            // 4. Save into VR Device Session Summary
            // - Only add into the database for those device is connected or even disconnected but already joined the room
            IEnumerable<ModelVRDevice> connectedDevices = modelVRLearningSession.Devices.Values.Where(d => d.IsAlreadyJoined);

            foreach (ModelVRDevice modelVrDevice in connectedDevices)
            {
                VRDevice vrDevice = await uow
                    .Repository<VRDevice>()
                    .GetEntityBySpec(new VRDevicesSpecification(serialNumber: modelVrDevice.VrDeviceSerialNumber));

                if (vrDevice == null)
                {
                    continue;
                }

                var vrDeviceSessionSummary = VRDeviceSessionSummary.Create(modelVrDevice, newVrLearningSession.Id, vrDevice.Id, currentDateUtcNow);
                uow.Repository<VRDeviceSessionSummary>().Add(vrDeviceSessionSummary);

                // 5. Save into the VRTaskProgress
                foreach (ModelTaskProgress modeltaskProgress in modelVrDevice.Tasks.Values)
                {
                    var dbProgress = VRDeviceTaskProgress.Create(
                        vrDevice.Id,
                        newVrLearningSession.Id,
                        modelVrDevice.StudentName,
                        currentDateUtcNow,
                        modeltaskProgress
                    );
                    uow.Repository<VRDeviceTaskProgress>().Add(dbProgress);
                }
            }

            // 6. Save Change
            await uow.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error occurs while saving session into the database: {Message}", ex.Message);
        }
    }
}
