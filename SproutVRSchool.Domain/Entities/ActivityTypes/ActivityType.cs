using SproutVRSchool.Domain.Entities.ObjectActivityTypes;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Domain.Entities.ActivityTypes;

public sealed class ActivityType : BaseEntity
{
    public string Name { get; set; }
    public string ActivityCode { get; set; }
    public string ConfigSchema { get; set; }

    // navigation property
    public ICollection<ObjectActivityType> ObjectActivityTypes { get; set; } = [];
    public ICollection<VRTask> VRTasks { get; set; } = [];
}
