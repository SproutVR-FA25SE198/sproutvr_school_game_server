using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Domain.Entities.Maps;

public sealed class Map : BaseEntity
{
    public Guid SubjectId { get; set; }
    public string MapCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string PreviewUrl { get; set; }
    public MapStatus Status { get; set; }

    // navigation property
    public Subject Subject { get; set; }
    public ICollection<TaskLocation> TaskLocations { get; set; } = [];
    public ICollection<MapObject> MapObjects { get; set; } = [];
}
