using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Application.Specifications;

public sealed class VRTaskSpecification : BaseSpecification<VRTask>
{
    public VRTaskSpecification(Guid vrLessonId)
        : base(x => x.VRLessonId == vrLessonId)
    {

    }
}
