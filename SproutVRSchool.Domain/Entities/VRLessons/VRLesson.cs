using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Domain.Entities.VRLessons;

public sealed class VRLesson : BaseEntity
{
    public Guid LessonId { get; set; }
    public Guid MapId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan MaxDuration { get; set; }
    public string? PresetJsonRelativeFilePath { get; set; }
    public VRLessonStatus Status { get; set; }
    public DateTimeOffset? PlayedAtUtc { get; set; }

    // navigation properties
    public Lesson Lesson { get; set; }
    public Map Map { get; set; }
    public ICollection<VRLearningSession> VRLearningSessions { get; set; } = [];
    public ICollection<VRTask> VRTasks { get; set; } = [];

    // ========================================
    // === Factory method
    // ========================================

    /// <summary>
    /// Create 
    /// </summary>
    /// <param name="lessonId"></param>
    /// <param name="mapId"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="maxDuration"></param>
    /// <returns></returns>
    public static VRLesson Create(
        Guid lessonId,
        Guid mapId,
        string name,
        string description,
        TimeSpan maxDuration)
    {
        return new VRLesson
        {
            Id = Guid.NewGuid(),
            LessonId = lessonId,
            MapId = mapId,
            Name = name,
            Description = description,
            MaxDuration = maxDuration,
            PresetJsonRelativeFilePath = string.Empty,
            Status = VRLessonStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    public void SetPresetFile(
        string presetJsonRelativeFilePath)
    {
        PresetJsonRelativeFilePath = presetJsonRelativeFilePath;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void SetPlayedAtTime()
    {
        PlayedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void UpdateStatus(VRLessonStatus status)
    {
        Status = status;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}


