using System.Collections.Concurrent;

namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelVRLearningSession
{
    public string VRLearningSessionId { get; set; }
    public string VRLessonId { get; set; }
    public string TeacherId { get; set; }
    public string? RoomCode { get; set; }
    public ModelVRLearningSessionStatus Status { get; set; } = ModelVRLearningSessionStatus.Pending;
    public int? DurationInMinutes { get; set; }
    public DateTimeOffset? StartTimeAtUtc { get; set; }
    public DateTimeOffset? EndTimeAtUtc { get; set; }
    public string? PresentJsonUrl { get; set; }
    public Dictionary<string, ModelVRDevice> Devices { get; set; } = new();
}
