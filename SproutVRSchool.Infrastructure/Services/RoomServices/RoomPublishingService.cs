using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.RoomServices.Publishers;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Domain;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

public class RoomPublishingService : IRoomPublishingService
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IDatabase _database;
    private readonly ILogger<RoomPublishingService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    // ===============================
    // === Constructor
    // ===============================

    public RoomPublishingService(
        IConnectionMultiplexer connectionMultiplexer,
        ILogger<RoomPublishingService> logger)
    {
        _database = connectionMultiplexer.GetDatabase();
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = false
        };
    }

    // ===============================
    // === Desktop Notifications
    // ===============================

    public async Task PublishRoomCancelledAsync(string vrLearningSessionId, RoomCancelledDto dto)
        => await PublishToDesktopChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.ROOM_CANCELLED, dto);

    public async Task PublishRoomEndedAsync(string vrLearningSessionId, RoomEndedDto dto)
        => await PublishToDesktopChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.ROOM_ENDED, dto);

    public async Task PublishDeviceJoinedAsync(string vrLearningSessionId, DeviceJoinedDto dto)
        => await PublishToDesktopChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.DEVICE_JOINED, dto);

    public async Task PublishDeviceDisconnectedAsync(string vrLearningSessionId, DeviceDisconnectedDto dto)
        => await PublishToDesktopChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.DEVICE_DISCONNECTED, dto);

    public async Task PublishTaskUpdatedAsync(string vrLearningSessionId, TaskUpdatedDto dto)
        => await PublishToDesktopChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.TASK_UPDATED, dto);

    // ===============================
    // === VR Device Notifications
    // ===============================

    public async Task PublishEndSessionAsync(string vrLearningSessionId, string reason)
        => await PublishToVRDeviceChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.END_SIGNAL, reason);

    public async Task PublishInfoAsync(string vrLearningSessionId, string message)
        => await PublishToVRDeviceChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.INFO, message);

    public async Task PublishWarningAsync(string vrLearningSessionId, string message)
        => await PublishToVRDeviceChannel(vrLearningSessionId, AppCts.Redis.PubSubEvents.WARNING, message);

    // ===============================
    // === Private Helper Methods
    // ===============================

    /// <summary>
    /// Publishes a DTO as a JSON payload to the global Desktop channel.
    /// </summary>
    private async Task PublishToDesktopChannel(string vrLearningSessionId, string eventType, object dto)
    {
        try
        {
            // 1. Serialize DTO to JSON
            string payloadJson = JsonSerializer.Serialize(dto, _jsonOptions);

            // 2. Get the Redis publisher
            ISubscriber subscriber = _database.Multiplexer.GetSubscriber();

            // 3. Format the Redis message string (e.g., "session-id:EVENT_TYPE:{...}")
            string redisEvent = vrLearningSessionId.ToRedisEventTypeMessage(eventType, payloadJson);

            // 4. Publish to the Desktop channel
            await subscriber.PublishAsync(
                RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS_TO_DESKTOP_CHANNEL),
                redisEvent);

            _logger.LogInformation("Published to Desktop Channel: {Event}", redisEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish DESKTOP event {EventType} for session {SessionId}", eventType, vrLearningSessionId);
        }
    }

    /// <summary>
    /// Publishes a simple string message to the global VR Device channel.
    /// </summary>
    private async Task PublishToVRDeviceChannel(string vrLearningSessionId, string eventType, string message)
    {
        try
        {
            // 1. Get the Redis publisher
            ISubscriber subscriber = _database.Multiplexer.GetSubscriber();

            // 2. Format the Redis message string (e.g., "session-id:END_SIGNAL:Time's up")
            string redisEvent = vrLearningSessionId.ToRedisEventTypeMessage(eventType, message);

            // 3. Publish to the VR channel
            await subscriber.PublishAsync(
                RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS_TO_VR_CHANNEL),
                redisEvent);

            _logger.LogInformation("Published to VR Channel: {Event}", redisEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish VR event {EventType} for session {SessionId}", eventType, vrLearningSessionId);
        }
    }
}
