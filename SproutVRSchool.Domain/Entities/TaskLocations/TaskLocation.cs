using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.ObjectLocations;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Domain.Entities.TaskLocations;

public sealed class TaskLocation : BaseEntity
{
    public Guid MapId { get; set; }
    public string LocationCode { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }

    // navigation property
    public Map Map { get; set; }
    public ICollection<VRTask> VRTasks { get; set; } = [];
    public ICollection<ObjectLocation> ObjectLocations { get; set; } = [];
}
