using SproutVRSchool.Application.Abstractions.RoomServices.Publishings;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Infrastructure.Services.RoomServices.Publishings;

public class RedisRoomMessagingNameService : IRoomChanneNameService, IRoomStreamNameService
{
    public string GetDesktopChannelNameOnVrLearningSessionId(string vrLearningSesisonId)
    {
        return $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_CHANNELS_NOTIFY_EVENTS_TO_DESKTOP}:{vrLearningSesisonId}";
    }

    public string GetVRDeviceChannelNameOnVrLearningSessionId(string vrLearningSesisonId)
    {
        return $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_CHANNELS_NOTIFY_EVENTS_TO_VR}:{vrLearningSesisonId}";
    }

    public string GetTaskUpdatedStreamName(string vrLearningSessionId)
    {
        return $"{AppCts.Redis.NAMESPACE_VR_LEARNING_SESSIONS_STREAMS_TASK_UPDATED_EVENTS}:{vrLearningSessionId}";
    }
}
