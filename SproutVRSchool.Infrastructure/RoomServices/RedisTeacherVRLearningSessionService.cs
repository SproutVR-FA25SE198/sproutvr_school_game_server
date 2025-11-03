using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using NRedisStack.RedisStackCommands;
using Polly;
using Polly.Registry;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Entities.VRTasks;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;
    private readonly ILogger<RedisTeacherVRLearningSessionService> _logger;

    // ===============================
    // === Constructors
    // ===============================

    public RedisTeacherVRLearningSessionService(
        ICodeGeneratorService codeGenerator,
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        ResiliencePipelineProvider<string> resiliencePipelineProvider,
        ILogger<RedisTeacherVRLearningSessionService> logger
        )
    {
        _codeGenerator = codeGenerator;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _pipelineProvider = resiliencePipelineProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _database = connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    // ===============================
    // === Methods
    // ===============================

    public async Task<CreateRoomResponseDto> CreateRoomAsync(CreateRoomRequestDto request)
    {
        // 1. Get essential variables
        VRLesson vrLesson = await _unitOfWork.Repository<VRLesson>().GetEntityByIdAsync(Guid.Parse(request.VrLessionId));

        // If not found, then throw back to the client
        if (vrLesson == null)
        {
            throw new SvrNotFoundException($"VR Lesson with ID '{request.VrLessionId}' not found.");
        }

        // 2. Create a model for injecting in redis
        var vrLearningSesison = new ModelVRLearningSession
        {
            VRLearningSessionId = Guid.NewGuid().ToString(),
            TeacherId = request.TeacherId,
            VRLessonId = request.VrLessionId,
            ClassName = request.ClassName,
            Status = ModelVRLearningSessionStatus.Pending,
        };

        // 3. Prefix for grouping keys
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSesison.VRLearningSessionId}";
        string jsonPayLoad = JsonSerializer.Serialize(vrLearningSesison, _jsonOptions);

        // 4. Set into the redis db
        await _database.JSON().SetAsync(sessionKey, "$", jsonPayLoad, When.NotExists);
        return new CreateRoomResponseDto(vrLearningSesison.VRLearningSessionId);
    }

    public async Task<ActivateRoomResponseDto> ActivateRoomAsync(ActivateRoomRequestDto request)
    {
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{request.LearningSessionId}";
        string roomCode = string.Empty;
        ResiliencePipeline<bool> retryPipeline = _pipelineProvider.GetPipeline<bool>(AppCts.RetryKeys.REDIS_TRANSACTION_KEY);

        // 1. Get vr lesson info for duration and task lists
        VRLesson vrLesson = await _unitOfWork.Repository<VRLesson>().GetEntityByIdAsync(Guid.Parse(request.VrLessonId));

        // If not found, then throw back to the client
        if (vrLesson == null)
        {
            throw new SvrNotFoundException($"VR Lesson with ID '{request.VrLessonId}' not found.");
        }

        // 2. Get the list tasks related to the VRLesson from repositories
        // Set Tasks params for the devices, by default isCompleted = false, isCorrect = false
        (IReadOnlyList<VRTask> Data, int Count) vrTasks = await _unitOfWork.Repository<VRTask>()
            .ListAsync(new VRTaskSpecification(vrLesson.Id));

        var taskTemplate = vrTasks.Data.ToDictionary(
            vrTask => vrTask.Id.ToString(),
            vrTask => new ModelTaskProgress
            {
                VRTaskId = vrTask.Id.ToString(),
                IsCompleted = false,
                IsCorrect = false,
                CompletionTimeAtUtc = null,
                Status = ModelTaskProgressStatus.Uncompleted
            });

        var initialDevices = request.AssignedDeviceSerials.ToDictionary(
            el => el.VrDeviceSerialNumber, el => new ModelVRDevice
            {
                VrDeviceSerialNumber = el.VrDeviceSerialNumber,
                StudentName = el.StudentName,
                Status = ModelVRDeviceStatus.Disconnected,
                Tasks = new ConcurrentDictionary<string, ModelTaskProgress>(taskTemplate)
            });

        // 3. Retry execute the activate if failed due to room code conflict
        bool isSuccess = await retryPipeline.ExecuteAsync<bool>(async (cancellationToken) =>
        {
            roomCode = _codeGenerator.GenerateCode();
            string roomCodeKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ROOM_CODE}:{roomCode}";

            ITransaction transaction = _database.CreateTransaction();

            // Set the roomcode -> vr_learning_session_id for lookup
            // only set if the room code is not exist, if already exist, then generate the other code
            _ = transaction.StringSetAsync(
                roomCodeKey,
                request.LearningSessionId,
                vrLesson.MaxDuration,
                When.NotExists);

            // Set into the active lists, but must be in the tranasction
            _ = transaction.SetAddAsync(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ACTIVE, request.LearningSessionId);

            // Set params to the room to Activate the room
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelVRLearningSessionStatus.Active, _jsonOptions));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.RoomCode", JsonSerializer.Serialize(roomCode));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.StartTimeAtUtc", JsonSerializer.Serialize(request.StartTimeUtc));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.DurationInSeconds", vrLesson.MaxDuration.TotalSeconds);
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Devices", JsonSerializer.Serialize(initialDevices, _jsonOptions));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.PresetJsonRelativeFilePath", JsonSerializer.Serialize(vrLesson.PresetJsonRelativeFilePath, _jsonOptions));

            return await transaction.ExecuteAsync();
        });

        if (!isSuccess)
        {
            throw new Exception("Failed to activate session. Please try again.");
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
            // UNDONE: Store to the DB
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
