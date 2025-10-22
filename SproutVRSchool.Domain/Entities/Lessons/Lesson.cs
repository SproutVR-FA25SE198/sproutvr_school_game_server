using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Domain.Entities.Lessons;

public sealed class Lesson : BaseEntity
{
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ResourceRelativeFilePath { get; set; }
    public LessonStatus Status { get; set; }

    // navigation property
    public Subject Subject { get; set; }
    public Teacher Teacher { get; set; }
    public ICollection<VRLesson> VRLessons { get; set; } = [];

    public static Lesson Create(
        Guid subjectId,
        Guid teacherId,
        string name,
        string description,
        string? resourceRelativeFilePath)
    {
        return new Lesson
        {
            Id = Guid.NewGuid(),
            SubjectId = subjectId,
            TeacherId = teacherId,
            Name = name,
            Description = description,
            ResourceRelativeFilePath = resourceRelativeFilePath ?? string.Empty,
            Status = LessonStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }
}
