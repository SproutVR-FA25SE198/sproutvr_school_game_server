using System.Text.Json;
using System.Text.Json.Serialization;
using LearningSession.V1;
using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;
using SproutVRSchool.Domain;
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

    // ============================
    // === Constructors
    // ============================

    public RedisTeacherVRLearningSessionStateService(
        IConnectionMultiplexer connectionMultiplexer,
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
    }

    // ============================
    // === Methods
    // ============================

    public IAsyncEnumerable<TeacherRoomUpdateResponseDto> StreamRoomUpdatesAsync(string vrLearningSessionId, CancellationToken cancellationToken)
    {
        return null;
    }

    public async Task<GetRoomStateResponse> GetRoomStateAsync(string vrLearningSessionId)
    {
        // 1. Get Key
        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{vrLearningSessionId}";
        RedisResult redisValue = await _database.ExecuteAsync("JSON.GET", sessionKey);

        GetRoomStateResponse? roomState = JsonSerializer.Deserialize<GetRoomStateResponse>(redisValue.ToString(), _jsonOptions);

        // UNDONE: implement the exception handling strategy
        if (roomState == null)
        {
            _logger.LogWarning("Room state not found for VR Learning Session ID: {VrLearningSessionId}", vrLearningSessionId);
            throw new KeyNotFoundException($"Room state not found for VR Learning Session ID: {vrLearningSessionId}");
        }

        return roomState;
    }
}
