using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Services.CodeGenerator;
using SproutVRSchool.Application.Abstractions.Services.TeacherSession;
using SproutVRSchool.Application.Abstractions.Services.TeacherSession.Dtos;
using SproutVRSchool.Application.Abstractions.Services.VRGlassSession;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services;

internal sealed class RedisTeacherVRLearningSessionService
    : IVRLearningSessionTeacherService
{
    private readonly IDatabase _database;
    private readonly ICodeGeneratorService _codeGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JsonSerializerOptions _jsonOptions;

    // ===============================
    // === Constructors
    // ===============================
    public RedisTeacherVRLearningSessionService(
        ICodeGeneratorService codeGenerator,
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider
        )
    {
        _codeGenerator = codeGenerator;
        _dateTimeProvider = dateTimeProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _database = connectionMultiplexer.GetDatabase();
    }

    // ===============================
    // === Methods
    // ===============================
    public async Task<CreateRoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request)
    {
        var vrLearningSesison = new ModelVRLearningSession
        {
            VRLearningSessionId = Guid.NewGuid().ToString(),
            TeacherId = request.TeacherId,
            VRLessonId = request.VrLessionId,
            Status = ModelVRLearningSessionStatus.Pending
        };

        // prefix for grouping keys
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{vrLearningSesison.VRLearningSessionId}";
        string jsonPayLoad = JsonSerializer.Serialize(vrLearningSesison, _jsonOptions);

        // set into the redis db
        await _database.JSON().SetAsync(sessionKey, "$", jsonPayLoad, When.NotExists);
        return new CreateRoomResponseDto(vrLearningSesison.VRLearningSessionId);
    }

    public async Task<ActivateRoomResponseDto> ActivateRoomAsync(ActivateRoomRequestDto request)
    {
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{request.LearningSessionId}";
        string roomCode = _codeGenerator.GenerateCode();
        string roomCodeKey = $"{AppCts.Session.NAMESPACE_ROOM_CODE}:{roomCode}";

        // Generate code for the room
        var initialDevices = request.AssignedDeviceSerials.ToDictionary(
            serialNumber => serialNumber, serialNumber => new ModelVRDevice
            {
                SerialNumber = serialNumber
            });

        ITransaction transaction = _database.CreateTransaction();

        // Set the roomcode -> vr_learning_session_id for lookup
        // only set if the room code is not exist, if already exist, then generate the other code
        _ = transaction.StringSetAsync(
            roomCodeKey,
            request.LearningSessionId,
            TimeSpan.FromMinutes(AppCts.Session.CODE_DURATION_IN_MINUTES),
            When.NotExists);

        // Set params to the room to Activate the room
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Active, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.RoomCode", JsonSerializer.Serialize(roomCode));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.StartTimeAtUtc", JsonSerializer.Serialize(request.StartTimeUtc));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.DurationInMinutes", request.DurationInMinutes);
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Devices", JsonSerializer.Serialize(initialDevices, _jsonOptions));

        if (!await transaction.ExecuteAsync())
        {
            // This can fail if the room code wasn't unique or the session key doesn't exist.
            // A retry loop could be added here for more robustness.
            throw new Exception("Failed to activate session. The generated room code might have conflicted or the session ID is invalid.");
        }

        return new ActivateRoomResponseDto(roomCode);
    }

    public async Task<CancelRoomResponseDto> CancelRoomAsync(string vrLearningSessionId)
    {
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{vrLearningSessionId}";

        if (!await _database.KeyExistsAsync(sessionKey))
        {
            // If this key not exit, just do nothing
            return new CancelRoomResponseDto(
                Message: $"Session with ID '{vrLearningSessionId}' not found. Canceled Failed.");
        }

        ITransaction transaction = _database.CreateTransaction();
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Cancelled, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.EndTimeAtUtc", JsonSerializer.Serialize(_dateTimeProvider.UtcDateTimeNow));

        if (!await transaction.ExecuteAsync())
        {
            return new CancelRoomResponseDto(
                Message: "Cancellation failed. Please try again");
        }

        return new CancelRoomResponseDto(
            Message: "Cancelled VR Learning Session Successfull"
            );
    }
}
