using SproutVRSchool.Application.Commons.Requests;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.SearchActivityTypes;

public sealed class AuthorizedSearchActivityTypesQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public string? ActivityCode { get; set; }
    public Guid? MapObjectId { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}
