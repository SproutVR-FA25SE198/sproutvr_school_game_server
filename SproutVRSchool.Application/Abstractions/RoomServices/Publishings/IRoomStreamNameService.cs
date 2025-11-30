namespace SproutVRSchool.Application.Abstractions.RoomServices.Publishings;

public interface IRoomStreamNameService
{
    /// <summary>
    /// Get the corresponding stream with vrLearningSessionId for TaskUpdate event
    /// </summary>
    /// <param name="vrLearningSessionId"></param>
    /// <returns></returns>
    string GetTaskUpdatedStreamName(string vrLearningSessionId);
}
