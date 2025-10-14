namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelLearningSession
{
    public string LearningSessionId { get; set; }
    public string VrLessonId { get; set; }
    public string TeacherId { get; set; }
    public string RoomCode { get; set; }
    public ModelLearningSessionStatus Status { get; set; }
    public DateTime? StartTimeAtUtc { get; set; }
    public int? DurationInMinutes { get; set; }
    public DateTime? EndTimeAtUtc { get; set; }
    public int MaxVrDevices { get; set; }
    public Dictionary<string, ModelVRDevice> Devices { get; set; }
}
