using SproutVRSchool.Application.Commons.Requests;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.SearchMapObjects;

public sealed class AuthorizedSearchMapObjectsQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public string? ObjectCode { get; set; }
    public string? MapId { get; set; }
    public string? TaskLocationId { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}

