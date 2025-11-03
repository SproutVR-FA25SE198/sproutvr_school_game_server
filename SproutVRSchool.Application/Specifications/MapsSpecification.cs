using SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Querries.SearchMaps;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.Specifications;

public sealed class MapsSpecification : BaseSpecification<Map>
{
    public MapsSpecification(AuthorizedSearchMapsQueryParams searchMapsParams)
        : base(x => (
            (string.IsNullOrEmpty(searchMapsParams.Name) || x.Name.Contains(searchMapsParams.Name)) &&
            (string.IsNullOrEmpty(searchMapsParams.MapCode) || x.MapCode.Contains(searchMapsParams.MapCode)) &&
            (!searchMapsParams.SubjectId.HasValue || x.SubjectId == searchMapsParams.SubjectId) &&
            (!searchMapsParams.MapStatus.HasValue || x.Status == searchMapsParams.MapStatus))
        )
    {
        // Pagination if enabled
        if (searchMapsParams.IsPaginated.HasValue && searchMapsParams.IsPaginated.Value && searchMapsParams.PageSize.HasValue && searchMapsParams.PageIndex.HasValue)
        {
            ApplyPaging(searchMapsParams.PageSize.Value * (searchMapsParams.PageIndex.Value - 1), searchMapsParams.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchMapsParams.SortBy))
        {
            searchMapsParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchMapsParams.SortBy)
        {
            case AppCts.SortingKeys.Maps.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.Maps.NAME_DESC:
                AddOrderByDescending(x => x.Name);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_ASC:
                AddOrderBy(x => x.UpdatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.UpdatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }

        // Includes
        AddInclude(x => x.Subject);
    }
}

