using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;

public sealed class VRDeviceSessionSummary : BaseEntity
{
    // ========================
    // === Fields
    // ========================

    public Guid VRDeviceId { get; set; }
    public Guid VRLearningSessionId { get; set; }
    public string StudentName { get; set; }
    public int NoTasksCompleted { get; set; }

    // Navigation properties
    public VRDevice VRDevice { get; set; }
    public VRLearningSession VRLearningSession { get; set; }

    // ========================
    // === Methods
    // ========================

    /// <summary>
    /// A functio tion to create a summary for the vr device session summary
    /// </summary>
    /// <param name="modelVrDevice"></param>
    /// <param name="vrLearningSessionId"></param>
    /// <param name="vrDeviceId"></param>
    /// <returns></returns>
    public static VRDeviceSessionSummary Create(ModelVRDevice modelVrDevice, Guid vrLearningSessionId, Guid vrDeviceId, DateTimeOffset currentDateUtcNow)
    {
        if (modelVrDevice.JoinedAtUtc == null)
        {
            throw new Exception("JoinedAtUtc cannot be null");
        }

        return new VRDeviceSessionSummary()
        {
            Id = Guid.NewGuid(),
            VRDeviceId = vrDeviceId,
            VRLearningSessionId = vrLearningSessionId,
            StudentName = modelVrDevice.StudentName,
            NoTasksCompleted = modelVrDevice.Tasks.Count(x => x.Value.IsCompleted),
            CreatedAtUtc = currentDateUtcNow,
            UpdatedAtUtc = currentDateUtcNow
        };
    }
}
