using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Domain.Entities.VRLearningSessions;

public sealed class VRLearningSession : BaseEntity
{
    public Guid VRLessonId { get; set; }
    public Guid TeacherId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public VRLearningSessionStatus Status { get; set; }

    // Navigation properties
    public VRLesson VRLesson { get; set; }
    public Teacher Teacher { get; set; }
    public ICollection<VRDeviceTaskProgress> VRDeviceTaskProgresses { get; set; } = [];
    public ICollection<VRDeviceSessionSummary> VRDeviceSessionSummaries { get; set; } = [];
}
