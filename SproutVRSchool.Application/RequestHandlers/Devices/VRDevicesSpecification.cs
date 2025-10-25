using SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices;

public sealed class VRDevicesSpecification : BaseSpecification<VRDevice>
{
    /// <summary>
    /// Searching VRDevices specification with filtering, sorting and pagination.
    /// </summary>
    /// <param name="searchVRDevicesParam"></param>
    public VRDevicesSpecification(SearchVRDevicesParams searchVRDevicesParam)
        : base(x =>
            (string.IsNullOrEmpty(searchVRDevicesParam.Name) || x.Name.Contains(searchVRDevicesParam.Name)) &&
            (string.IsNullOrEmpty(searchVRDevicesParam.SerialNumber) || x.SerialNumber.Contains(searchVRDevicesParam.SerialNumber)) &&
            (!searchVRDevicesParam.VRDeviceStatus.HasValue || x.Status == searchVRDevicesParam.VRDeviceStatus))
    {
        // Pagination
        if (searchVRDevicesParam.IsPaginated)
        {
            ApplyPaging(searchVRDevicesParam.PageSize * (searchVRDevicesParam.PageIndex - 1), searchVRDevicesParam.PageSize);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchVRDevicesParam.SortBy))
        {
            searchVRDevicesParam.SortBy = SearchVRDevicesCts.DefaultSort;
        }

        switch (searchVRDevicesParam.SortBy)
        {
            case SearchVRDevicesCts.NameAsc:
                AddOrderBy(x => x.Name);
                break;
            case SearchVRDevicesCts.NameDesc:
                AddOrderByDescending(x => x.Name);
                break;
            case SearchVRDevicesCts.CreatedAtUtcAsc:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case SearchVRDevicesCts.CreatedAtUtcDesc:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            case SearchVRDevicesCts.UpdatedAtUtcAsc:
                AddOrderBy(x => x.UpdatedAtUtc);
                break;
            case SearchVRDevicesCts.UpdatedAtUtcDesc:
                AddOrderByDescending(x => x.UpdatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }

    /// <summary>
    /// Find specific details of the device by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="macAddress"></param>
    public VRDevicesSpecification(Guid id)
     : base(x => x.Id == id)
    {
        AddInclude(d => d.VRDeviceSessionSummaries);
        AddInclude(d => d.VRDeviceTaskProgresses);
    }
}
