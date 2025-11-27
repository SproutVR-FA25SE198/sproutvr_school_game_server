using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Domain.Entities.ObjectLocations;

public sealed class ObjectLocation : BaseEntity
{
    public Guid ObjectId { get; set; }
    public Guid TaskLocationId { get; set; }

    // navigation property
    public TaskLocation TaskLocation { get; set; }
    public MapObject MapObject { get; set; }
}
