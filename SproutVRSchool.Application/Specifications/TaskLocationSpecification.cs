using SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.SearchTaskLocations;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Application.Specifications;

public sealed class TaskLocationsSpecification : BaseSpecification<TaskLocation>
{
    public TaskLocationsSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Map);
    }

    public TaskLocationsSpecification(AuthorizedSearchTaskLocationsQueryParams searchParams)
            : base(x =>
                (!searchParams.MapId.HasValue || x.MapId == searchParams.MapId.Value) &&
                (string.IsNullOrEmpty(searchParams.LocationCode) || x.LocationCode.Contains(searchParams.LocationCode)) &&
                (string.IsNullOrEmpty(searchParams.Name) || x.Name.Contains(searchParams.Name)))
    {
        // Pagination
        if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
        {
            ApplyPaging(searchParams.PageSize.Value * (searchParams.PageIndex.Value - 1), searchParams.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchParams.SortBy))
        {
            searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchParams.SortBy)
        {
            case AppCts.SortingKeys.TaskLocations.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.TaskLocations.NAME_DESC:
                AddOrderByDescending(x => x.Name);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }

        // Include related Map
        AddInclude(x => x.Map);
    }
}
