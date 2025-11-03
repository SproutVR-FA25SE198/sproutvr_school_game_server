using SproutVRSchool.Application.Commons.Requests;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.SearchTaskLocations;

public class AuthorizedSearchTaskLocationsQueryParams : BaseGetListParams
{
    public Guid? MapId { get; set; }
    public string? LocationCode { get; set; }
    public string? Name { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}
