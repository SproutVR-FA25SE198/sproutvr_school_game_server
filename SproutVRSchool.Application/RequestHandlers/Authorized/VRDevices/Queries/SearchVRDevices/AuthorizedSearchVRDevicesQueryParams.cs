using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.SearchVRDevices;

public sealed class AuthorizedSearchVRDevicesQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public VRDeviceStatus? VRDeviceStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}

