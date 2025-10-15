
using System.Text.Json;
using System.Text.Json.Serialization;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.RoomSession;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Redis.VRLearningSession;

public class VRLearningSessionImpl : IRoomSession
{
    // ===========================
    // === Fields
    // ===========================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ICodeGenerator _codeGenerator;
    private readonly IRoomSessionValidator _validator;

    // ===========================
    // === Constructors
    // ===========================

    public VRLearningSessionImpl(
        IConnectionMultiplexer connectionMultiplexer,
        ICodeGenerator codeGenerator,
        IRoomSessionValidator validator)
    {
        _database = connectionMultiplexer.GetDatabase();
        _codeGenerator = codeGenerator;
        _validator = validator;
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
    }

    // ===========================
    // === Methods
    // ===========================

    public async Task<CreateRoomResult> CreateRoomAsync(Guid ownerId, Guid? vrLessionId, string sessionName, int maxVrDevices)
    {
        // session info
        var session = new ModelLearningSession
        {
            LearningSessionId = Guid.NewGuid().ToString(),
            VrLessonId = vrLessionId.ToString() ?? "",
            TeacherId = ownerId.ToString(),
            RoomCode = _codeGenerator.GenerateCode(),
            Status = ModelLearningSessionStatus.Pending,
            StartTimeAtUtc = DateTime.UtcNow,
            MaxVrDevices = maxVrDevices
        };

        // save session object to in redis
        await CreateRoomInRedis(session);
        return new CreateRoomResult(session.LearningSessionId, session.RoomCode);
    }

    public async Task<string> EndRoomAsync(string roomId)
    {
        // 1. Get the session id
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{roomId}";

        // 2. check if room session exists or not
        if (!await _validator.IsRoomSessionExistsAsync(roomId))
        {
            throw new Exception($"Session with ID '{roomId}' not found.");
        }

        // 3. update the status to avoid student rejoin the game
        ITransaction transaction = _database.CreateTransaction();
        await transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelLearningSessionStatus.Completed, _jsonOptions));
        await transaction.ExecuteAsync("JSON.SET", sessionKey, "$.EndTimeAtUtc", JsonSerializer.Serialize(DateTime.UtcNow, _jsonOptions));

        if (!await transaction.ExecuteAsync())
        {
            throw new Exception($"Failed to end session '{roomId}'.");
        }

        return roomId;
    }

    public Task<JoinRoomResult> JoinRoomAsync(string roomCode, string deviceIdentifier, string deviceName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Setting time for the room and activate the room
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="startTimeUtc"></param>
    /// <param name="durationInMinutes"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<CreateRoomResult> ScheduleRoomAsync(string roomId, DateTime startTimeUtc, int durationInMinutes)
    {
        // 1. get the session key
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{roomId}";

        // 2. check if room session exists or not
        if (!await _validator.IsRoomSessionExistsAsync(roomId))
        {
            throw new Exception($"Session with ID '{roomId}' not found.");
        }

        // 3. create a transaction to update the created room
        // - dont use await for parallel updation
        ITransaction transaction = _database.CreateTransaction();
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.Status", JsonSerializer.Serialize(ModelLearningSessionStatus.Active, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.StartTimeAtUtc", JsonSerializer.Serialize(startTimeUtc, _jsonOptions));
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$.DurationInMinutes", durationInMinutes);

        // 4. set back those tiem values
        if (!await transaction.ExecuteAsync())
        {
            throw new Exception($"Failed to schedule session '{roomId}'.");
        }

        // 5. get back the room code and roomId for clients
        RedisResult roomCodeResult = await _database.JSON().GetAsync(sessionKey, path: "$.RoomCode");
        string roomCode = roomCodeResult.Length > 0 ? roomCodeResult.ToString() : string.Empty;

        return new CreateRoomResult(roomId, roomCode);
    }

    // ===========================
    // === Private Helper Methods
    // ===========================
    private async Task CreateRoomInRedis(ModelLearningSession session)
    {
        // 1. create keys
        string sessionKey = $"{AppCts.Session.NAMESPACE_VR_LEARNING_SESSION}:{session.LearningSessionId}";
        string roomCode = $"{AppCts.Session.NAMESPACE_ROOM_CODE}:{session.RoomCode}";
        string jsonPayload = JsonSerializer.Serialize(session, _jsonOptions);

        // 2. Set the session object
        ITransaction transaction = _database.CreateTransaction();
        _ = transaction.ExecuteAsync("JSON.SET", sessionKey, "$", jsonPayload);

        // 3. Set a code pointer pointing to learning session for fast key retrieve
        _ = transaction.StringSetAsync(
            new RedisKey(roomCode),
            new RedisValue(session.LearningSessionId),
            TimeSpan.FromMinutes(AppCts.Session.CODE_DURATION_IN_MINUTES));

        // 4. store those object
        if (await transaction.ExecuteAsync())
        {
            return;
        }

        // Catch exception here
        throw new Exception("Không thể tạo session với mã phòng duy nhất sau nhiều lần thử.");
    }
}
