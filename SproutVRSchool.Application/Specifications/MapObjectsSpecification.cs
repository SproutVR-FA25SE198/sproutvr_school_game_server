using SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.SearchMapObjects;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.MapObjects;

namespace SproutVRSchool.Application.Specifications;

public sealed class MapObjectsSpecification : BaseSpecification<MapObject>
{
    public MapObjectsSpecification(AuthorizedSearchMapObjectsQueryParams param)
        : base(x =>
            (string.IsNullOrEmpty(param.Name) || x.Name.Contains(param.Name)) &&
            (string.IsNullOrEmpty(param.ObjectCode) || x.ObjectCode.Contains(param.ObjectCode)) &&
            (string.IsNullOrEmpty(param.MapId) || x.MapId.ToString() == param.MapId) &&
            (string.IsNullOrEmpty(param.TaskLocationId) ||
                x.ObjectLocations.Any(ol => ol.TaskLocationId.ToString() == param.TaskLocationId))
        )
    {
        if (param.IsPaginated!.Value && param.PageSize.HasValue && param.PageIndex.HasValue)
        {
            ApplyPaging(param.PageSize.Value * (param.PageIndex.Value - 1), param.PageSize.Value);
        }

        if (string.IsNullOrEmpty(param.SortBy))
        {
            param.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (param.SortBy)
        {
            case AppCts.SortingKeys.MapObjects.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.MapObjects.NAME_DESC:
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

        AddInclude(x => x.Map);
        AddInclude(x => x.ObjectLocations);
    }
}

