using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.Maps;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Querries.SearchMaps;

public class AuthorizedSearchMapsQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public Guid? SubjectId { get; set; }
    public string? MapCode { get; set; }
    public MapStatus? MapStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}

