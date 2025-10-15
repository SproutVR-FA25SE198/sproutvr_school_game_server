using System.Collections.Concurrent;

namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelVRLearningSession
{
    public string LearningSessionId { get; set; }
    public string VrLessonId { get; set; }
    public string TeacherId { get; set; }
    public string RoomCode { get; set; }
    public ModelVRLearningSessionStatus Status { get; set; } = ModelVRLearningSessionStatus.Pending;
    public int? DurationInMinutes { get; set; }
    public DateTime? StartTimeAtUtc { get; set; }
    public DateTime? EndTimeAtUtc { get; set; }
    public string? PresentJsonContentUrl { get; set; }
    public Dictionary<string, ModelVRDevice> Devices { get; set; } = new();
}
