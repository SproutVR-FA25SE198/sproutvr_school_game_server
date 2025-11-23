using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

namespace SproutVRSchool.Application.Abstractions.RoomServices.Publishers;

public interface IRoomPublishingService
{
    // ===============================
    // === Desktop Notifications 
    // ===============================

    Task PublishRoomCancelledAsync(string vrLearningSessionId, RoomCancelledDto dto);
    Task PublishRoomEndedAsync(string vrLearningSessionId, RoomEndedDto dto);
    Task PublishDeviceJoinedAsync(string vrLearningSessionId, DeviceJoinedDto dto);
    Task PublishDeviceDisconnectedAsync(string vrLearningSessionId, DeviceDisconnectedDto dto);
    Task PublishTaskUpdatedAsync(string vrLearningSessionId, TaskUpdatedDto dto);

    // ===============================
    // === VR Device Notifications 
    // ===============================

    Task PublishEndSessionAsync(string vrLearningSessionId, string reason);
    Task PublishInfoAsync(string vrLearningSessionId, string message);
    Task PublishWarningAsync(string vrLearningSessionId, string message);
}
