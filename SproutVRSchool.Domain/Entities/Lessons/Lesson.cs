using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Domain.Entities.Lessons;

public sealed class Lesson : BaseEntity
{
    // ===========================
    // === Fields
    // ===========================

    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ResourceRelativeFilePath { get; set; }
    public LessonStatus Status { get; set; }

    // navigation property
    public Subject Subject { get; init; }
    public Teacher Teacher { get; init; }
    public ICollection<VRLesson> VRLessons { get; init; } = [];

    // ===========================
    // === Methods
    // ===========================

    /// <summary>
    /// Create a new VR Lesson
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="subjectId"></param>
    /// <param name="teacherId"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="resourceRelativeFilePath"></param>
    /// <returns></returns>
    public static Lesson Create(
        Guid Id,
        Guid subjectId,
        Guid teacherId,
        string name,
        string description,
        string? resourceRelativeFilePath)
    {
        return new Lesson
        {
            Id = Id,
            SubjectId = subjectId,
            TeacherId = teacherId,
            Name = name,
            Description = description,
            ResourceRelativeFilePath = resourceRelativeFilePath ?? string.Empty,
            Status = LessonStatus.Active,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Updating fields
    /// </summary>
    /// <param name="newName"></param>
    /// <param name="newDescription"></param>
    /// <param name="newResourceRelativeFilePath"></param>
    public void Update(
        string? newName,
        string? newDescription,
        string? newResourceRelativeFilePath)
    {
        Name = string.IsNullOrWhiteSpace(newName) ? Name : newName;

        Description = string.IsNullOrWhiteSpace(newDescription) ? Description : newDescription;

        ResourceRelativeFilePath = string.IsNullOrWhiteSpace(newResourceRelativeFilePath)
            ? ResourceRelativeFilePath
            : newResourceRelativeFilePath;

        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }
}
