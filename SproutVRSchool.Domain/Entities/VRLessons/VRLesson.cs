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
    public string? PresetJsonRelativeFilePath { get; set; }
    public string ImageRelativeFilePath { get; set; }
    public VRLessonStatus Status { get; set; }

    // navigation properties
    public Lesson Lesson { get; set; }
    public Map Map { get; set; }
    public ICollection<VRLearningSession> VRLearningSessions { get; set; } = [];

    // ========================================
    // === Factory method
    // ========================================

    public static VRLesson Create(
        Guid lessonId,
        Guid mapId,
        string name,
        string description,
        TimeSpan maxDuration,
        string imageRelativeFilePath)
    {
        return new VRLesson
        {
            Id = Guid.NewGuid(),
            LessonId = lessonId,
            MapId = mapId,
            Name = name,
            Description = description,
            MaxDuration = maxDuration,
            ImageRelativeFilePath = imageRelativeFilePath,
            PresetJsonRelativeFilePath = string.Empty,
            Status = VRLessonStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }
}


