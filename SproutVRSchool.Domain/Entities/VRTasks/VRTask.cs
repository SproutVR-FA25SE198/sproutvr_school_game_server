using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.TaskLocations;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;

namespace SproutVRSchool.Domain.Entities.VRTasks;

public sealed class VRTask : BaseEntity
{
    public Guid TaskLocationId { get; set; }
    public Guid MapObjectId { get; set; }
    public Guid ActivityTypeId { get; set; }
    public string TaskNumber { get; set; }
    public string Description { get; set; }

    // navigation properties
    public TaskLocation TaskLocation { get; set; }
    public MapObject MapObject { get; set; }
    public ActivityType ActivityType { get; set; }
    public ICollection<VRDeviceTaskProgress> DeviceTaskProgresses { get; set; } = [];
}
