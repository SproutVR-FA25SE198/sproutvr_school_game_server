using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Lessons;

internal sealed class LessonSpecification : BaseSpecification<Lesson>
{
    /// <summary>
    /// Get the lesson by id
    /// </summary>
    /// <param name="id"></param>
    public LessonSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Subject);
        AddInclude(x => x.Teacher);
    }
}
