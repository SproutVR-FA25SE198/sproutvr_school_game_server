using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.RoomServices;

internal sealed class RoomValidator : IVRLearningSessionValidator
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;

    // ===============================
    // === Constructors
    // ===============================

    public RoomValidator(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
    }

    // ===============================
    // === Methods
    // ===============================

    public async Task<ValidationResult> ValidateJoinAttemptAsync(string roomCode, string deviceSerialNumber)
    {
        string sessionId = await _database.StringGetAsync($"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ROOM_CODE}:{roomCode}");

        // 1. If type code is invalid or learning session is not exist under the room code
        if (string.IsNullOrEmpty(sessionId))
        {
            return ValidationResult.InvalidRoomCode;
        }

        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{sessionId}";
        RedisResult sessionJson = await _database.JSON().GetAsync(sessionKey);

        // 2. Room not found, or session expired
        if (sessionJson.IsNull)
        {
            return ValidationResult.SessionExpiredOrNotFound;
        }

        ModelVRLearningSession vrLearningSession = JsonSerializer.Deserialize<ModelVRLearningSession>(sessionJson.ToString()!, _jsonOptions)!;

        // 3. VR glasses cannot join the room at Pending, Completed, Cancelled
        if (vrLearningSession.Status != ModelVRLearningSessionStatus.Active)
        {
            return ValidationResult.SessionNotActive;
        }

        // 4. Device not assigned to the session, cannot join
        if (vrLearningSession.Devices == null || !vrLearningSession.Devices.ContainsKey(deviceSerialNumber))
        {
            return ValidationResult.DeviceNotAssigned;
        }

        // 5. Device already connected
        if (vrLearningSession.Devices.TryGetValue(deviceSerialNumber, out ModelVRDevice? existingModelVRDevice)
            && existingModelVRDevice.Status == ModelVRDeviceStatus.Connected)
        {
            return ValidationResult.AlreayJoined;
        }

        // 5. Ok if passing all of those validation, return success with metadata
        return ValidationResult.Success(
            vrLearningSessionId: vrLearningSession.VRLearningSessionId,
            presetJsonContent: string.Empty,
            modelVRLearningSession: vrLearningSession
        );
    }
}
