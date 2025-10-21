using System.Text.Json;
using System.Text.Json.Serialization;
using NRedisStack.RedisStackCommands;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Models.VRLearningSession;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.RoomServices;

internal sealed class RoomValidator : IVRLearningSessionValidator
{
    // ===============================
    // === Fields
    // ===============================

    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;

    // ===============================
    // === Constructors
    // ===============================

    public RoomValidator(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
    }

    // ===============================
    // === Methods
    // ===============================

    public async Task<ValidationResult> ValidateJoinAttemptAsync(string roomCode, string deviceSerialNumber)
    {
        string sessionId = await _database.StringGetAsync($"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_ROOM_CODE}:{roomCode}");

        // 1. If type code is invalid or learning session is not exist under the room code
        if (string.IsNullOrEmpty(sessionId))
        {
            return ValidationResult.InvalidRoomCode;
        }

        string sessionKey = $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS}:{sessionId}";
        RedisResult sessionJson = await _database.JSON().GetAsync(sessionKey);

        // 2. Room not found, or session expired
        if (sessionJson.IsNull)
        {
            return ValidationResult.SessionExpiredOrNotFound;
        }

        ModelVRLearningSession vrLearningSession = JsonSerializer.Deserialize<ModelVRLearningSession>(sessionJson.ToString()!, _jsonOptions)!;

        // 3. VR glasses cannot join the room at Pending, Completed, Cancelled
        if (vrLearningSession.Status != ModelVRLearningSessionStatus.Active)
        {
            return ValidationResult.SessionNotActive;
        }

        // 4. Device not assigned to the session, cannot join
        if (vrLearningSession.Devices == null || !vrLearningSession.Devices.ContainsKey(deviceSerialNumber))
        {
            return ValidationResult.DeviceNotAssigned;
        }

        // 5. Device already connected
        if (vrLearningSession.Devices.TryGetValue(deviceSerialNumber, out ModelVRDevice? existingModelVRDevice)
            && existingModelVRDevice.Status == ModelVRDeviceStatus.Connected)
        {
            return ValidationResult.AlreayJoined;
        }

        // 5. Ok if passing all of those validation, return success with metadata
        return ValidationResult.Success(
            vrLearningSessionId: vrLearningSession.VRLearningSessionId,
            presetJsonContent: "\"{\\r\\n  \\\"mapCode\\\": \\\"map_test_1\\\",\\r\\n  \\\"duration\\\": 300.00,\\r\\n  \\\"isSequential\\\": true,\\r\\n  \\\"vrTasks\\\": [\\r\\n    {\\r\\n      \\\"locationCode\\\":\\\"map_test_1_loc_1\\\",\\r\\n      \\\"vrTaskId\\\": \\\"adb1f931-574a-4eab-affe-a1e20aa5bf33\\\",\\r\\n      \\\"taskNumber\\\": 2,\\r\\n      \\\"taskDescription\\\": \\\"(Cầm) Ống dung dịch CuSO4\\\",\\r\\n      \\\"mapObject\\\": {\\r\\n        \\\"objectCode\\\": \\\"tube_cuso4\\\",\\r\\n        \\\"activityType\\\": {\\r\\n          \\\"activityCode\\\": \\\"grab\\\",\\r\\n        }\\r\\n      }\\r\\n    },\\r\\n    {\\r\\n      \\\"locationCode\\\": \\\"map_test_1_loc_2\\\",\\r\\n      \\\"vrTaskId\\\": \\\"660753a9-3a17-4a7c-a363-8e286c1d382c\\\",\\r\\n      \\\"taskNumber\\\": 3,\\r\\n      \\\"taskDescription\\\": \\\"(Câu hỏi) Ống chứa đinh sắt gỉ\\\",\\r\\n      \\\"mapObject\\\": {\\r\\n        \\\"objectCode\\\": \\\"tube_rusty\\\",\\r\\n        \\\"activityType\\\": {\\r\\n          \\\"activityCode\\\": \\\"quiz\\\",\\r\\n          \\\"config\\\": {\\r\\n            \\\"question\\\": \\\"Phương pháp chống ăn mòn kim loại dựa vào thí nghiệm đinh sắt quấn dây kẽm là?\\\",\\r\\n            \\\"answers\\\": [\\r\\n              { \\\"text\\\": \\\"Phương pháp phủ bề mặt\\\", \\\"isCorrect\\\": false },\\r\\n              { \\\"text\\\": \\\"Phương pháp điện hóa\\\", \\\"isCorrect\\\": true },\\r\\n              { \\\"text\\\": \\\"Phương pháp trùng ngưng\\\", \\\"isCorrect\\\": false },\\r\\n              { \\\"text\\\": \\\"Phương pháp thủy nhiệt\\\", \\\"isCorrect\\\": false }\\r\\n            ]\\r\\n          }\\r\\n        }\\r\\n      }\\r\\n    },\\r\\n    {\\r\\n      \\\"locationCode\\\": \\\"map_test_1_loc_3\\\",\\r\\n      \\\"vrTaskId\\\": \\\"7f0064b1-8766-4789-95a5-05301f778b20\\\",\\r\\n      \\\"taskNumber\\\": 4,\\r\\n      \\\"taskDescription\\\": \\\"(Thông tin) Ống chứa dung dịch CuSO4 và đinh sắt\\\",\\r\\n      \\\"mapObject\\\": {\\r\\n        \\\"objectCode\\\": \\\"tube_cuso4_nail\\\",\\r\\n        \\\"activityType\\\": {\\r\\n          \\\"activityCode\\\": \\\"info\\\",\\r\\n          \\\"config\\\": {\\r\\n            \\\"information\\\": \\\"Khi cho đinh sắt vào dung dịch CuSO4 màu xanh lam thì dung dịch sẽ nhạt màu dần, trên đinh sắt xuất hiện chất rắn màu nâu đỏ bám vào. Hiện tượng này xảy ra do Fe có tính hoạt động hóa học mạnh hơn Cu, đẩy Cu ra khỏi dung dịch muối và tạo dung dịch muối mới là FeSO4.\\\"\\r\\n          }\\r\\n        }\\r\\n      }\\r\\n    },\\r\\n    {\\r\\n      \\\"locationCode\\\": \\\"map_test_1_loc_4\\\",\\r\\n      \\\"vrTaskId\\\": \\\"37487095-c9af-4d08-af03-0aff1504faea\\\",\\r\\n      \\\"taskNumber\\\": 1,\\r\\n      \\\"taskDescription\\\": \\\"(Chọn) Cốc dung dịch H2SO4\\\",\\r\\n      \\\"mapObject\\\": {\\r\\n        \\\"objectCode\\\": \\\"beaker_h2so4\\\",\\r\\n        \\\"activityType\\\": {\\r\\n          \\\"activityCode\\\": \\\"interact\\\"\\r\\n        }\\r\\n      }\\r\\n    },\\r\\n  ]\\r\\n}\\r\\n\\r\\n\\r\\n\\r\\n\\r\\n\\r\\n\\r\\n\\r\\n\"", // To be filled later
            modelVRLearningSession: vrLearningSession
        );
    }
}
