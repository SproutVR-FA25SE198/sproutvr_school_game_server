using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public class SearchVRDevicesParams : PagingParams
{
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public VRDeviceStatus? VRDeviceStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}

