using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.TaskLocations;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Domain.Entities.VRTasks;

public sealed class VRTask : BaseEntity
{
    public Guid TaskLocationId { get; set; }
    public Guid MapObjectId { get; set; }
    public Guid ActivityTypeId { get; set; }
    public Guid VRLessonId { get; set; }
    public int TaskNumber { get; set; }
    public string Description { get; set; }

    // navigation properties
    public TaskLocation TaskLocation { get; set; }
    public MapObject MapObject { get; set; }
    public ActivityType ActivityType { get; set; }
    public VRLesson VRLesson { get; set; }
    public ICollection<VRDeviceTaskProgress> DeviceTaskProgresses { get; set; } = [];

    // =============================
    // === Methods
    // =============================

    public static VRTask Create(
        Guid taskLocationId,
        Guid mapObjectId,
        Guid activityTypeId,
        Guid vrLessonId,
        int taskNumber,
        string description)
    {
        var newVRTask = new VRTask
        {
            Id = Guid.NewGuid(),
            TaskLocationId = taskLocationId,
            MapObjectId = mapObjectId,
            ActivityTypeId = activityTypeId,
            VRLessonId = vrLessonId,
            TaskNumber = taskNumber,
            Description = description,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        return newVRTask;
    }

}
