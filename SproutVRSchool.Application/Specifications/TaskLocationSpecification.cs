using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Application.Specifications;

public sealed class TaskLocationsSpecification : BaseSpecification<TaskLocation>
{
    public TaskLocationsSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Map);
    }
}
