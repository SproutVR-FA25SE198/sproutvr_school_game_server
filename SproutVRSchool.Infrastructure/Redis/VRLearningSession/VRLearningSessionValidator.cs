using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.RoomSession;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;
using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Infrastructure.Redis.VRLearningSession;

public class VRLearningSessionValidator : IRoomSessionValidator
{
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;

    public VRLearningSessionValidator(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
    }

    /// <summary>
    /// This function is for validating after room code being validated
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="deviceIdentifier"></param>
    /// <returns></returns>
    public async Task<ValidationResult> ValidateLearningSessionRoomAsync(string roomId, string deviceIdentifier)
    {
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{roomId}";
        RedisResult sessionData = await _database.JSON().GetAsync(sessionKey);

        // No room
        if (sessionData.IsNull)
        {
            return new ValidationResult(false, null, new JoinRoomResult(JoinStatus.RoomNotFound, "This session could not be found."));
        }

        string sessionJson = sessionData.ToString()!;
        ModelLearningSession session = JsonSerializer.Deserialize<ModelLearningSession>(sessionJson, _jsonOptions);

        // Has room, but not active
        if (session!.Status != ModelLearningSessionStatus.Pending)
        {
            return new ValidationResult(false, null, new JoinRoomResult(JoinStatus.ServerError, "This session is not available to join."));
        }

        // Has room, but session finished
        if (session.StartTimeAtUtc.HasValue && DateTimeOffset.UtcNow < session.StartTimeAtUtc.Value)
        {
            return new ValidationResult(false, null, new JoinRoomResult(JoinStatus.ServerError, "This session has not started yet."));
        }

        // Has room, but full devices
        if (session.Devices.Count >= session.MaxVrDevices)
        {
            return new ValidationResult(false, null, new JoinRoomResult(JoinStatus.ServerError, "This session is full."));
        }

        // Has room, device already joined
        if (session.Devices.ContainsKey(deviceIdentifier))
        {
            return new ValidationResult(true, null, new JoinRoomResult(JoinStatus.ServerError, "Already joined the room."));
        }

        // return if passing all validation
        return new ValidationResult(true, roomId);
    }

    /// <summary>
    /// Functio to check room code
    /// </summary>
    /// <param name="roomCode"></param>
    /// 
    /// <returns></returns>
    public async Task<ValidationResult> ValidateRoomCodeAsync(string roomCode)
    {
        // Get the sessionId based on the room code
        RedisValue learningSessionId = await _database.StringGetAsync($"{AppCts.Session.NAMESPACE_ROOM_CODE}:{roomCode}");
        if (string.IsNullOrEmpty(learningSessionId))
        {
            return new ValidationResult(false, null, new JoinRoomResult(
                Status: JoinStatus.WrongCode,
                "Invalid or expired room code."
                ));
        }

        // return if passing all validation
        return new ValidationResult(true, learningSessionId);
    }
}
