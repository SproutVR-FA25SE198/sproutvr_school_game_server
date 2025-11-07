using SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.SearchVRDevices;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.Specifications;

public sealed class VRDevicesSpecification : BaseSpecification<VRDevice>
{
    /// <summary>
    /// Searching VRDevices specification with filtering, sorting and pagination.
    /// </summary>
    /// <param name="searchVRDevicesParam"></param>
    public VRDevicesSpecification(AuthorizedSearchVRDevicesQueryParams searchVRDevicesParam)
        : base(x =>
            (string.IsNullOrEmpty(searchVRDevicesParam.Name) || x.Name.Contains(searchVRDevicesParam.Name)) &&
            (string.IsNullOrEmpty(searchVRDevicesParam.SerialNumber) || x.SerialNumber.Contains(searchVRDevicesParam.SerialNumber)) &&
            (!searchVRDevicesParam.VRDeviceStatus.HasValue || x.Status == searchVRDevicesParam.VRDeviceStatus))
    {
        // If pagination is true, apply pagination
        if (searchVRDevicesParam.IsPaginated!.Value && searchVRDevicesParam.PageSize.HasValue && searchVRDevicesParam.PageIndex.HasValue)
        {
            ApplyPaging(searchVRDevicesParam.PageSize.Value * (searchVRDevicesParam.PageIndex.Value - 1), searchVRDevicesParam.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchVRDevicesParam.SortBy))
        {
            searchVRDevicesParam.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchVRDevicesParam.SortBy)
        {
            case AppCts.SortingKeys.VRDevices.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.VRDevices.NAME_DESC:
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

    /// <summary>
    /// Find specific VR device by device name and serial number
    /// </summary>
    /// <param name="deviceName"></param>
    /// <param name="serialNumber"></param>
    public VRDevicesSpecification(string deviceName, string serialNumber)
        : base(x => x.Name == deviceName && x.SerialNumber == serialNumber)
    {
    }
}
