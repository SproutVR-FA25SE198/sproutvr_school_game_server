using SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.SearchActivityTypes;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ActivityTypes;

namespace SproutVRSchool.Application.Specifications;

public sealed class ActivityTypesSpecification : BaseSpecification<ActivityType>
{
    public ActivityTypesSpecification(AuthorizedSearchActivityTypesQueryParams searchParams)
        : base(x =>
            (string.IsNullOrEmpty(searchParams.Name) || x.Name.Contains(searchParams.Name)) &&
            (string.IsNullOrEmpty(searchParams.ActivityCode) || x.ActivityCode.Contains(searchParams.ActivityCode)) &&
            (!searchParams.MapObjectId.HasValue ||
                x.ObjectActivityTypes.Any(oat => oat.MapObjectId == searchParams.MapObjectId.Value)))
    {
        // Pagination
        if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
        {
            ApplyPaging(searchParams.PageSize.Value * (searchParams.PageIndex.Value - 1),
                        searchParams.PageSize.Value);
        }

        // Sorting (only by Name)
        if (string.IsNullOrEmpty(searchParams.SortBy))
        {
            searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchParams.SortBy)
        {
            case AppCts.SortingKeys.ActivityTypes.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.ActivityTypes.NAME_DESC:
                AddOrderByDescending(x => x.Name);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }

        AddInclude(x => x.ObjectActivityTypes);
    }
}
