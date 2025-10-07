using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;

public sealed class VRDeviceSessionSummary : BaseEntity
{
    public Guid VRDeviceId { get; set; }
    public Guid VRLearningSessionId { get; set; }
    public string StudentName { get; set; }
    public int NoTasksCompleted { get; set; }

    // Navigation properties
    public VRDevice VRDevice { get; set; }
    public VRLearningSession VRLearningSession { get; set; }
}
