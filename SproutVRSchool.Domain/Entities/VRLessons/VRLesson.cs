using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Domain.Entities.VRLessons;

public sealed class VRLesson : BaseEntity
{
    public Guid LessonId { get; set; }
    public Guid MapId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan MaxDuration { get; set; }
    public string PresetJsonUrl { get; set; }
    public string ImageUrl { get; set; }
    public VRLessonStatus Status { get; set; }


    // navigation properties
    public Lesson Lesson { get; set; }
    public Map Map { get; set; }
    public ICollection<VRLearningSession> VRLearningSessions { get; set; } = [];
}
