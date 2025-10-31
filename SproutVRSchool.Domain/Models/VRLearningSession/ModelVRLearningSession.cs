namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelVRLearningSession
{
    public string VRLearningSessionId { get; set; }
    public string VRLessonId { get; set; }
    public string TeacherId { get; set; }
    public string ClassName { get; set; }
    public string? RoomCode { get; set; }
    public int? DurationInMinutes { get; set; }
    public DateTimeOffset? StartTimeAtUtc { get; set; }
    public DateTimeOffset? EndTimeAtUtc { get; set; }
    public string? PresetJsonRelativeFilePath { get; set; }
    public Dictionary<string, ModelVRDevice> Devices { get; set; } = new();
    public ModelVRLearningSessionStatus Status { get; set; } = ModelVRLearningSessionStatus.Pending;
}
