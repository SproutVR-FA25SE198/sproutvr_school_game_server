using System.Collections.Concurrent;

namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelLearningSession
{
    public string LearningSessionId { get; set; }
    public string VrLessonId { get; set; }
    public string TeacherId { get; set; }
    public string RoomCode { get; set; }
    public ModelLearningSessionStatus Status { get; set; } = ModelLearningSessionStatus.Pending;
    public int MaxVrDevices { get; set; }
    public int? DurationInMinutes { get; set; }
    public DateTime? StartTimeAtUtc { get; set; }
    public DateTime? EndTimeAtUtc { get; set; }
    public string? PresentJsonContentUrl { get; set; }
    public ConcurrentDictionary<string, ModelVRDevice> Devices { get; set; }
}
