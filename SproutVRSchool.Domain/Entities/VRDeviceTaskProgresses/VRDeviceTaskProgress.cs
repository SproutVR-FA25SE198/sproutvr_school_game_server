using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;

public sealed class VRDeviceTaskProgress : BaseEntity
{
    public Guid VRDeviceId { get; set; }
    public Guid VRTaskId { get; set; }
    public Guid VRLearningSessionId { get; set; }
    public string StudentName { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCorrect { get; set; }
    public DateTimeOffset CompletionTimeAtUtc { get; set; }

    // Navigation properties
    public VRDevice VRDevice { get; set; }
    public VRTask VRTask { get; set; }
    public VRLearningSession VRLearningSession { get; set; }
}
