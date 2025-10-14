using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.MapObjects;

namespace SproutVRSchool.Domain.Entities.ObjectActivityTypes;

public sealed class ObjectActivityType : BaseEntity
{
    public Guid MapObjectId { get; set; }
    public Guid ActivityTypeId { get; set; }

    // navigation property
    public MapObject MapObject { get; set; }
    public ActivityType ActivityType { get; set; }
}
