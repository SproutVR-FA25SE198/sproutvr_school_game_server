using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Domain.Entities.VRLearningSessions;

public sealed class VRLearningSession : BaseEntity
{
    public Guid VRLessonId { get; set; }
    public Guid TeacherId { get; set; }
    public string ClassName { get; set; }
    public DateTimeOffset StartTimeAtUtc { get; set; }
    public DateTimeOffset EndTimeAtUtc { get; set; }
    public int DurationInMinutes { get; set; }
    public VRLearningSessionStatus Status { get; set; }

    // Navigation properties
    public VRLesson VRLesson { get; set; }
    public Teacher Teacher { get; set; }
    public ICollection<VRDeviceTaskProgress> VRDeviceTaskProgresses { get; set; } = [];
    public ICollection<VRDeviceSessionSummary> VRDeviceSessionSummaries { get; set; } = [];

    /// <summary>
    /// A function to create a vr learning session
    /// </summary>
    /// <param name="modelVRLearningSession"></param>
    /// <returns></returns>
    public static VRLearningSession Create(ModelVRLearningSession modelVRLearningSession, DateTimeOffset currentDateUtcNow)
    {
        // Check for null StartTime
        if (modelVRLearningSession.StartTimeAtUtc == null)
        {
            throw new Exception("StartTimeAtUtc cannot be null when creating a session.");
        }

        // Check for null EndTime
        if (modelVRLearningSession.EndTimeAtUtc == null)
        {
            throw new Exception("EndTimeAtUtc cannot be null when creating a session.");
        }

        // Check for logical time error
        if (modelVRLearningSession.EndTimeAtUtc < modelVRLearningSession.StartTimeAtUtc)
        {
            throw new Exception("EndTimeAtUtc cannot be earlier than StartTimeAtUtc.");
        }

        // Room duration is null
        if (modelVRLearningSession.RoomDurationInMinutes == null)
        {
            throw new Exception("RoomDurationInMinutes cannot be null.");
        }

        return new VRLearningSession()
        {
            Id = Guid.Parse(modelVRLearningSession.VRLearningSessionId),
            VRLessonId = Guid.Parse(modelVRLearningSession.VRLessonId),
            TeacherId = Guid.Parse(modelVRLearningSession.TeacherId),
            ClassName = modelVRLearningSession.ClassName,

            StartTimeAtUtc = modelVRLearningSession.StartTimeAtUtc.Value,
            EndTimeAtUtc = modelVRLearningSession.EndTimeAtUtc.Value,
            DurationInMinutes = modelVRLearningSession.RoomDurationInMinutes.Value,
            Status = (VRLearningSessionStatus)modelVRLearningSession.Status,
            CreatedAtUtc = currentDateUtcNow,
            UpdatedAtUtc = currentDateUtcNow
        };
    }
}
