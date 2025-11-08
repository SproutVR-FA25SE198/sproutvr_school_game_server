using System.Text.Json;
using System.Text.Json.Serialization;
using LearningSession.V1;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Lessons;
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
    private readonly IUnitOfWork _uow;
    private readonly UserManager<UserAccount> _userManager;

    // ============================
    // === Constructors
    // ============================

    public RedisTeacherVRLearningSessionStateService(
        IConnectionMultiplexer connectionMultiplexer,
        IUnitOfWork uow,
        UserManager<UserAccount> userManager,
        ILogger<RedisTeacherVRLearningSessionStateService> logger
        )
    {
        _database = connectionMultiplexer.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        _logger = logger;
        _userManager = userManager;
        _uow = uow;
    }

    // ============================
    // === Methods
    // ============================

    public IAsyncEnumerable<TeacherRoomUpdateResponseDto> StreamRoomUpdatesAsync(string vrLearningSessionId, CancellationToken cancellationToken)
    {
        return null;
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
            DurationInSeconds = jsonObject.DurationInSeconds ?? 0,
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
                }).ToList()
            }).ToList()
        };

        _logger.LogInformation("GetRoomStateAsync: Retrieved room state for VRLearningSessionId: {VRLearningSessionId}", getRoomStateRequestDto.VRLearningSessionId);

        return resultDto;
    }

}
