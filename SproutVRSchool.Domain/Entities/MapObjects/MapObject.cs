using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.ObjectActivityTypes;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Domain.Entities.MapObjects;

public sealed class MapObject : BaseEntity
{
    public Guid MapId { get; set; }
    public string ObjectCode { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }

    // navigation property
    public Map Map { get; set; }
    public ICollection<ObjectActivityType> ObjectActivityTypes { get; set; } = [];

}
