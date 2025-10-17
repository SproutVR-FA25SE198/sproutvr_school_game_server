using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

internal class RoomValidator : IVRLearningSessionValidator
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
        string sessionId = await _database.StringGetAsync($"{AppCts.Session.NAMESPACE_ROOM_CODE}:{roomCode}");

        // 1. If type code is invalid or learning session is not exist under the room code
        if (string.IsNullOrEmpty(sessionId))
        {
            return ValidationResult.InvalidRoomCode;
        }

        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{sessionId}";
        string sessionJson = await _database.JSON().GetAsync(sessionKey, "$");

        // 2. Room not found, or session expired
        if (sessionJson == null)
        {
            return ValidationResult.SessionExpiredOrRoomNotFound;
        }

        ModelVRLearningSession vrLearningSession = JsonSerializer.Deserialize<ModelVRLearningSession>(sessionJson!, _jsonOptions)!;

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

        // 5. Ok if passing all of those validation, return success with metadata
        return ValidationResult.Success(
            vrLearningSessionId: vrLearningSession.VRLearningSessionId,
            modelVRLearningSession: vrLearningSession
        );
    }
}
