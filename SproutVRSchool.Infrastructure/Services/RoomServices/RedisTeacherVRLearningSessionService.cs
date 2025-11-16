using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using NRedisStack.RedisStackCommands;
using Polly;
using Polly.Registry;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;
using SproutVRSchool.Application.Abstractions.RoomServices.Publishers;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Entities.VRTasks;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

internal sealed class RedisTeacherVRLearningSessionService
    : ITeacherVRLearningSessionService
{
    private readonly IDatabase _database;
    private readonly ICodeGeneratorService _codeGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IServerPublishingService _serverPublishingService;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;

    // ===============================
    // === Constructors
    // ===============================

    public RedisTeacherVRLearningSessionService(
        ICodeGeneratorService codeGenerator,
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider,
        IServerPublishingService serverPublishingService,
        IUnitOfWork unitOfWork,
        ResiliencePipelineProvider<string> resiliencePipelineProvider
        )
    {
        _codeGenerator = codeGenerator;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _serverPublishingService = serverPublishingService;
        _resiliencePipelineProvider = resiliencePipelineProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _database = connectionMultiplexer.GetDatabase();
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
            throw new SvrResourceNotFoundException($"VR Lesson with ID '{request.VrLessionId}' not found.");
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
        ResiliencePipeline<bool> retryPipeline = _resiliencePipelineProvider.GetPipeline<bool>(AppCts.RetryKeys.REDIS_TRANSACTION_KEY);

        // 1. Get vr lesson info for duration and task lists
        VRLesson vrLesson = await _unitOfWork.Repository<VRLesson>().GetEntityByIdAsync(Guid.Parse(request.VrLessonId));

        // If not found, then throw back to the client
        if (vrLesson == null)
        {
            throw new SvrResourceNotFoundException($"VR Lesson with ID '{request.VrLessonId}' not found.");
        }

        // 2. Get the list tasks related to the VRLesson from repositories
        // - Set Tasks params for the devices, by default isCompleted = false, isCorrect = false
        // - Get the list of initial device as well
        (IReadOnlyList<VRTask> Data, int Count) vrTasks = await _unitOfWork.Repository<VRTask>()
            .ListAsync(new VRTasksSpecification(new ActivateRoomParams(vrLesson.Id)));

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
                IsAlreadyJoined = false,
                Tasks = new ConcurrentDictionary<string, ModelTaskProgress>(taskTemplate)
            });

        DateTimeOffset startTimeNowAtUtc = _dateTimeProvider.UtcDateTimeNow;
        DateTimeOffset endTimeNowAtUtc = startTimeNowAtUtc.AddMinutes(request.RoomDurationInMinutes);

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

            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.RoomDurationInSeconds", _dateTimeProvider.ConvertMinutesToSeconds(request.RoomDurationInMinutes));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.GameDurationInSeconds", vrLesson.MaxDuration.TotalSeconds);

            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.StartTimeAtUtc", JsonSerializer.Serialize(startTimeNowAtUtc, _jsonOptions));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.EndTimeAtUtc", JsonSerializer.Serialize(endTimeNowAtUtc, _jsonOptions));

            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Devices", JsonSerializer.Serialize(initialDevices, _jsonOptions));
            _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.PresetJsonRelativeFilePath", JsonSerializer.Serialize(vrLesson.PresetJsonRelativeFilePath, _jsonOptions));

            return await transaction.ExecuteAsync();
        });

        // UNDONE:
        // - Handle validation on interceptor
        if (!isSuccess)
        {
            throw new Exception("Failed to activate session. Please try again.");
        }

        return new ActivateRoomResponseDto(roomCode);
    }

    public async Task<CancelRoomResponseDto> CancelRoomAsync(string vrLearningSessionId)
    {
        // 1. Get vr learning session key
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSessionId}";

        if (!await _database.KeyExistsAsync(sessionKey))
        {
            // If this key not exit, just do nothing
            return new CancelRoomResponseDto(
                Message: $"Redis with ID '{vrLearningSessionId}' not found. Canceled Failed.");
        }

        // 2. Create a transaction to update the status and remove from active set
        // 3. Reset the preset end room to the utc datetime now
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

        // 3. Publish to VR Devices ENDSIGNAL event on VR Device Channel
        await _serverPublishingService.PublishEndSessionAsync(
            vrLearningSessionId,
            "The teacher has cancelled the session.");

        // 4. Publish to Desktop App Room State ROOMCANCELLED event on Desktop Channel
        await _serverPublishingService.PublishRoomCancelledAsync(
            vrLearningSessionId,
            new RoomCancelledDto()
        );

        // 5. Save the entire room state into the Physical DB for record keeping

        // 6. Return success message back to the teacher
        return new CancelRoomResponseDto(
            Message: "Cancelled VR Learning Redis Successfull"
        );
    }

    public async Task SendNotificationAsync(SendNotificationRequestDto request)
    {
        if (request.Severity.ToString().Equals(AppCts.Redis.PubSubEvents.WARNING, StringComparison.OrdinalIgnoreCase))
        {
            await _serverPublishingService.PublishWarningAsync(
                request.VRLearningSessionId,
                request.Text);
        }
        else
        {
            await _serverPublishingService.PublishInfoAsync(
                request.VRLearningSessionId,
                request.Text);
        }
    }
}
