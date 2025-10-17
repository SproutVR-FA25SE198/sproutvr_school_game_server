using System.Text.Json;
using System.Text.Json.Serialization;
using LearningSession.V1;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

public sealed class RedisVRGlassVRLearningSessionService : IVRLearningSessionWithVRGlassService
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IVRLearningSessionValidator _validator;

    // ===============================
    // === Constructors
    // ===============================

    public RedisVRGlassVRLearningSessionService(
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider,
        IVRLearningSessionValidator validator)
    {
        _database = connectionMultiplexer.GetDatabase();
        _dateTimeProvider = dateTimeProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _validator = validator;
    }

    // ===============================
    // === Methods
    // ===============================

    public async Task<JoinRoomResponseDto> JoinRoomAsync(JoinRoomRequestDto joinRoomRequestDto)

    {
        // 1. Validate the join attempt
        ValidationResult validation = await _validator.ValidateJoinAttemptAsync(
            joinRoomRequestDto.RoomCode,
            joinRoomRequestDto.VrDeviceSerialNumber
        );

        // 2. Return early if not valid
        if (!validation.IsValid)
        {
            return validation.JoinRoomResponseDto;
        }

        // Reuse the vr learning session object from the validator, if all passing
        ModelVRLearningSession vrLearningSession = validation.ModelVRLearningSession!;
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSION}:{vrLearningSession.VRLearningSessionId}";

#pragma warning disable S125 // Validation check rejoin. Will do later
        //UNDONE 3. Check rejoin
        //if (vrLearningSession.Devices.TryGetValue(joinRoomRequestDto.VrDeviceSerialNumber, out var existingVrDevice)
        //{

        //}
#pragma warning restore S125 //

        // 4. Update parallely the device info after passing all validation and connecting to the server
        ITransaction transaction = _database.CreateTransaction();
        string deviceRedisPath = $"$.Devices['{joinRoomRequestDto.VrDeviceSerialNumber}']";

        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{deviceRedisPath}.DeviceName", JsonSerializer.Serialize(joinRoomRequestDto.VrDeviceName));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{deviceRedisPath}.Status", JsonSerializer.Serialize(ModelVRDeviceStatus.Connected, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{deviceRedisPath}.JoinedAtUtc", JsonSerializer.Serialize(_dateTimeProvider.UtcDateTimeNow, _jsonOptions));

        if (!await transaction.ExecuteAsync())
        {
            throw new Exception($"Failed to update device status for session '{vrLearningSession.VRLearningSessionId}'.");
        }

        //UNDONE: 5. Stringtify the PresetJsonUrl and added
        // - validation at here is success
        validation.JoinRoomResponseDto!.PresetJsonContent = string.Empty;

        return validation.JoinRoomResponseDto!;
    }

    public Task PublishTaskUpdateToStreamAsync(PublishTaskUpdateRequestDto publishTaskUpdateRequestDto)
    {
        string streamKey = $"{AppCts.Redis.NAMESPACE_STREAM_EVENT_VR_LEARNING_SESSION}:{publishTaskUpdateRequestDto.VrLearningSessionId}";

        var eventPayload = new NameValueEntry[]
        {
            new NameValueEntry("VrDeviceSerialNumber", publishTaskUpdateRequestDto.VrDeviceSerialNumber),
            new NameValueEntry("VrLearningSessionId", publishTaskUpdateRequestDto.VrLearningSessionId.ToString()),
            new NameValueEntry("VrTaskId", publishTaskUpdateRequestDto.VrTaskId.ToString()),
            new NameValueEntry("IsCompleted", publishTaskUpdateRequestDto.IsCompleted),
            new NameValueEntry("IsCorrect", publishTaskUpdateRequestDto.IsCorrect),
            new NameValueEntry("EventType", "TaskUpdate")
        };

        return _database.StreamAddAsync(streamKey, eventPayload);
    }
}
