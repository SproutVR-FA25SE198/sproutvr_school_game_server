using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.Specifications;

internal sealed class VRLessonSpecification : BaseSpecification<VRLesson>
{
    public VRLessonSpecification(Guid vrLessonId)
        : base(x => x.Id == vrLessonId)
    {
        AddInclude(vrlesson => vrlesson.Map);
        AddInclude(vrLesson => vrLesson.Lesson);
        AddThenInclude(vrLesson => vrLesson.Include(j => j.VRTasks).ThenInclude(j => j.MapObject));
        AddThenInclude(vrLesson => vrLesson.Include(j => j.VRTasks).ThenInclude(j => j.ActivityType));
        AddThenInclude(vrLesson => vrLesson.Include(j => j.VRTasks).ThenInclude(j => j.TaskLocation));
    }
}
