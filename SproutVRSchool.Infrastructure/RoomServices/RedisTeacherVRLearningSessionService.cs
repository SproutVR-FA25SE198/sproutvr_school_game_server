using System.Text.Json;
using System.Text.Json.Serialization;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.RoomServices;

internal sealed class RedisTeacherVRLearningSessionService
    : IVRLearningSessionTeacherService
{
    private readonly IDatabase _database;
    private readonly ICodeGeneratorService _codeGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<RedisTeacherVRLearningSessionService> _logger;

    // ===============================
    // === Constructors
    // ===============================

    public RedisTeacherVRLearningSessionService(
        ICodeGeneratorService codeGenerator,
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        ILogger<RedisTeacherVRLearningSessionService> logger
        )
    {
        _codeGenerator = codeGenerator;
        _dateTimeProvider = dateTimeProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _database = connectionMultiplexer.GetDatabase();
        _logger = logger;
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
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSesison.VRLearningSessionId}";
        string jsonPayLoad = JsonSerializer.Serialize(vrLearningSesison, _jsonOptions);

        // set into the redis db
        await _database.JSON().SetAsync(sessionKey, "$", jsonPayLoad, When.NotExists);
        return new CreateRoomResponseDto(vrLearningSesison.VRLearningSessionId);
    }

    public async Task<ActivateRoomResponseDto> ActivateRoomAsync(ActivateRoomRequestDto request)
    {
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{request.LearningSessionId}";
        string roomCode = _codeGenerator.GenerateCode();
        string roomCodeKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ROOM_CODE}:{roomCode}";

        // UNDONE:
        // Get the list tasks related to the VRLesson from repositories
        // Set Tasks params for the devices, by default isCompleted = false, isCorrect = false
        var fakeTasksForLesson = new List<ModelTaskProgress>
        {
            new ModelTaskProgress { VRTaskId = "task-uuid-01", IsCompleted = false, IsCorrect = false, Status = ModelTaskProgressStatus.Uncompleted},
            new ModelTaskProgress { VRTaskId = "task-uuid-02", IsCompleted = false, IsCorrect = false, Status = ModelTaskProgressStatus.Uncompleted},
            new ModelTaskProgress { VRTaskId = "task-uuid-03", IsCompleted = false, IsCorrect = false, Status = ModelTaskProgressStatus.Uncompleted}
        };

        // Generate code for the room
        var initialDevices = request.AssignedDeviceSerials.ToDictionary(
            serialNumber => serialNumber, serialNumber => new ModelVRDevice
            {
                SerialNumber = serialNumber,
                Status = ModelVRDeviceStatus.Disconnected,
                Tasks = new System.Collections.Concurrent.ConcurrentDictionary<string, ModelTaskProgress>(
                    fakeTasksForLesson.ToDictionary(
                        task => task.VRTaskId,
                        task => task
                    )
                )
            });

        ITransaction transaction = _database.CreateTransaction();

        // Set the roomcode -> vr_learning_session_id for lookup
        // only set if the room code is not exist, if already exist, then generate the other code
        _ = transaction.StringSetAsync(
            roomCodeKey,
            request.LearningSessionId,
            TimeSpan.FromMinutes(AppCts.Redis.CODE_DURATION_IN_MINUTES),
            When.NotExists);

        // Set into the active lists, but must be in the tranasction
        _ = transaction.SetAddAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE, request.LearningSessionId);

        // Set params to the room to Activate the room
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Active, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.RoomCode", JsonSerializer.Serialize(roomCode));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.StartTimeAtUtc", JsonSerializer.Serialize(request.StartTimeUtc));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.DurationInMinutes", request.DurationInMinutes);
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Devices", JsonSerializer.Serialize(initialDevices, _jsonOptions));

        // UNDONE: Set the PresetJsonRelativeFilePath extract from the database
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.PresetJsonRelativeFilePath", JsonSerializer.Serialize("/presets/money", _jsonOptions));

        if (!await transaction.ExecuteAsync())
        {
            // UNDONE: Improve the error handling and retry mechanism
            // This can fail if the room code wasn't unique or the session key doesn't exist.
            // A retry loop could be added here for more robustness.
            throw new Exception("Failed to activate session. The generated room code might have conflicted or the session ID is invalid.");
        }

        return new ActivateRoomResponseDto(roomCode);
    }

    public async Task<CancelRoomResponseDto> CancelRoomAsync(string vrLearningSessionId)
    {
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSessionId}";

        if (!await _database.KeyExistsAsync(sessionKey))
        {
            // If this key not exit, just do nothing
            return new CancelRoomResponseDto(
                Message: $"Redis with ID '{vrLearningSessionId}' not found. Canceled Failed.");
        }

        ITransaction transaction = _database.CreateTransaction();
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Cancelled, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.EndTimeAtUtc", JsonSerializer.Serialize(_dateTimeProvider.UtcDateTimeNow));
        _ = transaction.SetRemoveAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE, vrLearningSessionId);

        if (!await transaction.ExecuteAsync())
        {
            // UNDONE: Improve the error handling and retry mechanism
            return new CancelRoomResponseDto(
                Message: "Cancellation failed. Please try again");
        }

        // Boardcasting the ENDSIGNAL message to all devices subscribed to the channel
        ISubscriber subscriber = _database.Multiplexer.GetSubscriber();
        string message = $"{vrLearningSessionId}:ENDSIGNAL:The teacher has cancelled the session.";
        await subscriber.PublishAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS), message);

        return new CancelRoomResponseDto(
            Message: "Cancelled VR Learning Redis Successfull"
        );
    }

    /// <summary>
    /// Send INFO or WARNING notification to all devices in the room using PUB/SUB
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task SendNotificationAsync(SendNotificationRequestDto request)
    {
        // Boardcasting the NOTIFY message to all devices subscribed to the channel
        ISubscriber subscriber = _database.Multiplexer.GetSubscriber();
        string message = $"{request.VRLearningSessionId}:{request.Severity}:{request.Text}";

        _logger.LogInformation("sending the message: {Message}", message);

        await subscriber.PublishAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS), message);
    }
}
