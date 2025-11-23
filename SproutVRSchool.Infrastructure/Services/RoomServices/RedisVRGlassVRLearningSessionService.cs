using System.Text.Json;
using System.Text.Json.Serialization;
using Polly;
using Polly.Registry;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.RoomServices.Publishers;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

public sealed class RedisVRGlassVRLearningSessionService : IVRGlassVRLearningSessionService
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILocalStorageService _localStorageService;
    private readonly IRoomValidator _validator;
    private readonly IRoomPublishingService _serverPublishingService;
    private readonly ResiliencePipelineProvider<string> _resiliencePipelineProvider;

    // ===============================
    // === Constructors
    // ===============================

    public RedisVRGlassVRLearningSessionService(
        IConnectionMultiplexer connectionMultiplexer,
        IDateTimeProvider dateTimeProvider,
        ILocalStorageService localStorageService,
        IRoomPublishingService serverPublishingService,
        ResiliencePipelineProvider<string> resiliencePipelineProvider,
        IRoomValidator validator)
    {
        _database = connectionMultiplexer.GetDatabase();
        _serverPublishingService = serverPublishingService;
        _dateTimeProvider = dateTimeProvider;
        _localStorageService = localStorageService;
        _resiliencePipelineProvider = resiliencePipelineProvider;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        _validator = validator;
    }

    // ===============================
    // === Methods
    // ===============================
    public async Task<JoinRoomResponseDto> JoinRoomAsync(JoinRoomRequestDto joinRoomRequestDto)
    {
        // 1. Validate the join attempt
        ValidationResultDto validation = await _validator.ValidateJoinAttemptAsync(
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
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSession.VRLearningSessionId}";

        // 3. Update parallely the device info after passing all validation and connecting to the server
        ITransaction transaction = _database.CreateTransaction();
        string deviceRedisPath = $"$.Devices['{joinRoomRequestDto.VrDeviceSerialNumber}']";

        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{deviceRedisPath}.Status", JsonSerializer.Serialize(ModelVRDeviceStatus.Connected, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{deviceRedisPath}.JoinedAtUtc", JsonSerializer.Serialize(_dateTimeProvider.UtcDateTimeNow, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, $"{deviceRedisPath}.IsAlreadyJoined", JsonSerializer.Serialize(true, _jsonOptions));

        if (!await transaction.ExecuteAsync())
        {
            throw new Exception($"Failed to update device status for session '{vrLearningSession.VRLearningSessionId}'.");
        }

        // 4. Stringtify the PresetJsonUrl and added
        string fileContent = await _localStorageService.LoadFileContentAsync(vrLearningSession.PresetJsonRelativeFilePath!);
        validation.JoinRoomResponseDto!.PresetJsonContent = fileContent;

        // 5. Publish DEVICEJOINED event to the Desktop App cjannel
        await _serverPublishingService.PublishDeviceJoinedAsync(
            vrLearningSession.VRLearningSessionId,
            new DeviceJoinedDto()
            {
                VrDeviceSerialNumber = joinRoomRequestDto.VrDeviceSerialNumber,
            }
        );

        return validation.JoinRoomResponseDto!;
    }

    public Task PublishTaskUpdateToStreamAsync(PublishTaskUpdateRequestDto publishTaskUpdateRequestDto)
    {
        string streamKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_STREAM_TASK_UPDATED_EVENTS}:{publishTaskUpdateRequestDto.VrLearningSessionId}";

        var eventPayload = new NameValueEntry[]
        {
            new NameValueEntry("VRDeviceSerialNumber", publishTaskUpdateRequestDto.VrDeviceSerialNumber),
            new NameValueEntry("VRLearningSessionId", publishTaskUpdateRequestDto.VrLearningSessionId.ToString()),
            new NameValueEntry("VRTaskId", publishTaskUpdateRequestDto.VrTaskId.ToString()),
            new NameValueEntry("IsCompleted", publishTaskUpdateRequestDto.IsCompleted),
            new NameValueEntry("IsCorrect", publishTaskUpdateRequestDto.IsCorrect),
            new NameValueEntry("EventType", "TaskUpdate")
        };

        // fire immediately, dont need to wait StreamAddAsync to finish
        // more performance for streaming cuz of the fire-and-forget nature
        // but also maintain the correct order of the function due to await the PublishTaskUpdateToStreamAsync
        return _database.StreamAddAsync(streamKey, eventPayload);
    }

    public async Task SetDeviceStatusDisconnectedAsync(string vrLearningSessionId, string vrDeviceSerialNumber)
    {
        // 1. Using poly incase setting failed
        ResiliencePipeline<bool> retryPipeline = _resiliencePipelineProvider.GetPipeline<bool>(AppCts.RetryKeys.REDIS_TRANSACTION_KEY);
        bool isSuccess = await retryPipeline.ExecuteAsync<bool>(async (cancellationToken) =>
        {

            // 1. Retrieve the device path
            string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSessionId}";
            string deviceRedisPath = $"$.Devices['{vrDeviceSerialNumber}']";

            // 2. Update Status to Disconnected
            ITransaction transaction = _database.CreateTransaction();
            _ = transaction.ExecuteAsync("JSON.SET",
                sessionKey,
                $"{deviceRedisPath}.Status",
                JsonSerializer.Serialize(ModelVRDeviceStatus.Disconnected, _jsonOptions));

            return await transaction.ExecuteAsync();
        });

        // UNDONE: set interceptor here
        if (!isSuccess)
        {
            throw new Exception($"Failed to set device serial {vrDeviceSerialNumber} to Disconnected");
        }

        // 2. If success, then publish to the desktop channel as well
        await _serverPublishingService.PublishDeviceDisconnectedAsync(
            vrLearningSessionId,
            new DeviceDisconnectedDto
            {
                VrDeviceSerialNumber = vrDeviceSerialNumber,
            });
    }
}
