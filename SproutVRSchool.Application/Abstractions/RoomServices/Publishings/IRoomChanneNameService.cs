namespace SproutVRSchool.Application.Abstractions.RoomServices.Publishings;

public interface IRoomChanneNameService
{
    /// <summary>
    /// Get the specific channel corresponding with the vrLearningSessionId for other events
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    string GetVRDeviceChannelNameOnVrLearningSessionId(string vrLearningSesisonId);

    /// <summary>
    /// Get the specific channel corresponding with the vrLearningSessionId for DesktopApp
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    string GetDesktopChannelNameOnVrLearningSessionId(string vrLearningSesisonId);
}
