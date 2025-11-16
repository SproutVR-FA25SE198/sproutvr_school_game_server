using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Services.RoomServices;

internal sealed class RedisTeacherVRLearningSessionStateService
    : ITeacherVRLearningSessionStateService
{
    // ============================
    // === Fields
    // ============================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<RedisTeacherVRLearningSessionStateService> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _uow;
    private readonly UserManager<UserAccount> _userManager;

    // ============================
    // === Constructors
    // ============================

    public RedisTeacherVRLearningSessionStateService(
        IConnectionMultiplexer connectionMultiplexer,
        IUnitOfWork uow,
        IDateTimeProvider dateTimeProvider,
        UserManager<UserAccount> userManager,
        ILogger<RedisTeacherVRLearningSessionStateService> logger
        )
    {
        _database = connectionMultiplexer.GetDatabase();
        _dateTimeProvider = dateTimeProvider;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = false
        };
        _logger = logger;
        _userManager = userManager;
        _uow = uow;
    }

    // ============================
    // === Methods
    // ============================

    public async IAsyncEnumerable<TeacherRoomUpdateResponseDto> StreamRoomUpdatesAsync(
        TeacherRoomUpdateRequestDto teacherRoomUpdateRequestDto,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // 1. Create a channel 
        ISubscriber subscriber = _database.Multiplexer.GetSubscriber();
        var channel = Channel.CreateUnbounded<RedisValue>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        // 2. Subscribe to Redis Pub/Sub channel
        await subscriber.SubscribeAsync(
         RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS_TO_DESKTOP_CHANNEL),
         (redisChannel, message) =>
         {
             if (!channel.Writer.TryWrite(message))
             {
                 _logger.LogWarning("Failed to enqueue Redis message for session {SessionId}", teacherRoomUpdateRequestDto.VrLearningSessionId);
             }
         });

        // 3. Read message (events) from the channel
        try
        {
            // apply coroutine to handle the event immediately incase of problem occurs
            await foreach (string redisMessage in channel.Reader.ReadAllAsync(cancellationToken))
            {
                // If redisMessage null, do nothing
                (string? identifier, string? eventType, string? message, bool isEventType) redisEvent = redisMessage.DeparseRedisEventMessage();

                if (!redisEvent.isEventType)
                {
                    continue;
                }

                // Only handle messages having the same identifier
                if (!string.Equals(redisEvent.identifier, teacherRoomUpdateRequestDto.VrLearningSessionId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Map to DTO based on event type
                TeacherRoomUpdateResponseDto? responseDto = null;
                switch (redisEvent.eventType)
                {
                    case AppCts.Redis.PubSubEvents.ROOM_CANCELLED:
                        {
                            RoomCancelledDto? dto = JsonSerializer.Deserialize<RoomCancelledDto>(redisEvent.message!);
                            responseDto = new TeacherRoomUpdateResponseDto
                            {
                                VrLearningSessionId = teacherRoomUpdateRequestDto.VrLearningSessionId,
                                RoomCancelled = dto
                            };
                            break;
                        }

                    case AppCts.Redis.PubSubEvents.ROOM_ENDED:
                        {
                            RoomEndedDto? dto = JsonSerializer.Deserialize<RoomEndedDto>(redisEvent.message!);
                            responseDto = new TeacherRoomUpdateResponseDto
                            {
                                VrLearningSessionId = teacherRoomUpdateRequestDto.VrLearningSessionId,
                                RoomEnded = dto
                            };
                            break;
                        }

                    case AppCts.Redis.PubSubEvents.DEVICE_JOINED:
                        {
                            DeviceJoinedDto? dto = JsonSerializer.Deserialize<DeviceJoinedDto>(redisEvent.message!);
                            responseDto = new TeacherRoomUpdateResponseDto
                            {
                                VrLearningSessionId = teacherRoomUpdateRequestDto.VrLearningSessionId,
                                DeviceJoined = dto
                            };
                            break;
                        }

                    case AppCts.Redis.PubSubEvents.DEVICE_DISCONNECTED:
                        {
                            DeviceDisconnectedDto? dto = JsonSerializer.Deserialize<DeviceDisconnectedDto>(redisEvent.message!);
                            responseDto = new TeacherRoomUpdateResponseDto
                            {
                                VrLearningSessionId = teacherRoomUpdateRequestDto.VrLearningSessionId,
                                DeviceDisconnected = dto
                            };
                            break;
                        }

                    case AppCts.Redis.PubSubEvents.TASK_UPDATED:
                        {
                            TaskUpdatedDto? dto = JsonSerializer.Deserialize<TaskUpdatedDto>(redisEvent.message!);
                            responseDto = new TeacherRoomUpdateResponseDto
                            {
                                VrLearningSessionId = teacherRoomUpdateRequestDto.VrLearningSessionId,
                                TaskUpdated = dto
                            };
                            break;
                        }

                    default:
                        {
                            _logger.LogWarning(
                                "Unhandled Redis event type: {EventType} for session {SessionId}",
                                redisEvent.eventType,
                                teacherRoomUpdateRequestDto.VrLearningSessionId);
                            break;
                        }
                }

                // If successfully mapped, yield the result immediately
                if (responseDto != null)
                {
                    yield return responseDto;
                }
            }
        }
        finally
        {
            await subscriber.UnsubscribeAsync(RedisChannel.Literal(AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_NOTIFY_EVENTS_TO_DESKTOP_CHANNEL));
        }
    }

    public async Task<GetRoomStateResponseDto> GetRoomStateAsync(GetRoomStateRequestDto getRoomStateRequestDto)
    {
        // 1. Get Key 
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{getRoomStateRequestDto.VRLearningSessionId}";
        RedisResult redisJson = await _database.ExecuteAsync("JSON.GET", sessionKey);

        // 2. If not found, then throw not found exception
        string json = redisJson.ToString();
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new SvrResourceNotFoundException($"Empty JSON for Room '{getRoomStateRequestDto.VRLearningSessionId}'");
        }

        // 3. get the dynamic typed object based on json
        ModelVRLearningSession jsonObject = JsonSerializer.Deserialize<ModelVRLearningSession>(json, _jsonOptions)
            ?? throw new Exception("Failed to deserialize VRLearningSession from Redis JSON");

        Teacher teacher = await _userManager.FindByIdAsync(jsonObject.TeacherId) as Teacher
            ?? throw new Exception("Teacher not found");

        VRLesson vrLesson = await _uow.Repository<VRLesson>().GetEntityByIdAsync(Guid.Parse(jsonObject.VRLessonId))
            ?? throw new Exception("Lesson not found");

        // 4. Map to DTO
        var resultDto = new GetRoomStateResponseDto
        {
            VRLearningSessionId = jsonObject.VRLearningSessionId,
            Teacher = new TeacherInfoDto
            {
                TeacherId = jsonObject.TeacherId,
                TeacherName = teacher.GetFullName()
            },

            VRLesson = new VRLessonInfoDto
            {
                VRLessonId = jsonObject.VRLessonId,
                Name = vrLesson.Name,
                Description = vrLesson.Description,
                PresetJsonRelativeFilePath = vrLesson.PresetJsonRelativeFilePath ?? string.Empty,
            },

            ClassName = jsonObject.ClassName,
            RoomDurationInSeconds = jsonObject.RoomDurationInSeconds ?? 0,
            GameDurationInSeconds = jsonObject.GameDurationInSeconds ?? 0,
            RoomCode = jsonObject.RoomCode ?? string.Empty,
            Status = jsonObject.Status.ToString(),
            Devices = jsonObject.Devices.Values.Select(device => new DeviceInfoDto
            {
                VRDeviceSerialNumber = device.VrDeviceSerialNumber,
                StudentName = device.StudentName,
                Status = device.Status.ToString(),
                Tasks = device.Tasks.Values.Select(task => new TaskInfoDto
                {
                    VRTaskId = task.VRTaskId,
                    IsCompleted = task.IsCompleted,
                    IsCorrect = task.IsCorrect,
                    Status = task.Status.ToString(),
                    CompletionTimeAtVietnam = _dateTimeProvider.ConvertToVietNamTime(task.CompletionTimeAtUtc)
                }).ToList()
            }).ToList()
        };

        _logger.LogInformation("GetRoomStateAsync: Retrieved room state for VRLearningSessionId: {VRLearningSessionId}", getRoomStateRequestDto.VRLearningSessionId);

        return resultDto;
    }
}
