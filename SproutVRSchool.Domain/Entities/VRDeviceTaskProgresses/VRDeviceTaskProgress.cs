using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Entities.VRTasks;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;

public sealed class VRDeviceTaskProgress : BaseEntity
{
    public Guid VRDeviceId { get; set; }
    public Guid VRTaskId { get; set; }
    public Guid VRLearningSessionId { get; set; }
    public string StudentName { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCorrect { get; set; }
    public DateTimeOffset? CompletionTimeAtUtc { get; set; }

    // Navigation properties
    public VRDevice VRDevice { get; set; }
    public VRTask VRTask { get; set; }
    public VRLearningSession VRLearningSession { get; set; }

    // =============================
    // === Methods
    // =============================

    /// <summary>
    /// A function to create the vr device task progress for each row in the summary
    /// </summary>
    /// <param name="vrDeviceId"></param>
    /// <param name="vrTaskId"></param>
    /// <param name="vrLearningSessionId"></param>
    /// <param name="studentName"></param>
    /// <param name="modelTaskProgress"></param>
    /// <returns></returns>
    public static VRDeviceTaskProgress Create(
        Guid vrDeviceId,
        Guid vrLearningSessionId,
        string studentName,
        DateTimeOffset currentDateUtcNow,
        ModelTaskProgress modelTaskProgress)
    {
        return new VRDeviceTaskProgress()
        {
            Id = Guid.NewGuid(),
            VRDeviceId = vrDeviceId,
            VRTaskId = Guid.Parse(modelTaskProgress.VRTaskId),
            VRLearningSessionId = vrLearningSessionId,
            StudentName = studentName,
            IsCompleted = modelTaskProgress.IsCompleted,
            IsCorrect = modelTaskProgress.IsCorrect,
            CompletionTimeAtUtc = modelTaskProgress.CompletionTimeAtUtc,
            CreatedAtUtc = currentDateUtcNow,
            UpdatedAtUtc = currentDateUtcNow
        };
    }
}
