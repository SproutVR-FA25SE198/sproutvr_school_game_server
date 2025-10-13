using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.Specifications;

public class VRDevicesSpecification : BaseSpecification<VRDevice>
{
    /// <summary>
    /// Searching VRDevices specification with filtering, sorting and pagination.
    /// </summary>
    /// <param name="searchVRDevicesParam"></param>
    public VRDevicesSpecification(SearchVRDevicesParams searchVRDevicesParam)
        : base(x =>
            (string.IsNullOrEmpty(searchVRDevicesParam.Name) || x.Name.Contains(searchVRDevicesParam.Name)) &&
            (string.IsNullOrEmpty(searchVRDevicesParam.SerialNumber) || x.Name.Contains(searchVRDevicesParam.SerialNumber) &&
            (!searchVRDevicesParam.VRDeviceStatus.HasValue || x.Status == searchVRDevicesParam.VRDeviceStatus)))
    {
        // Pagination
        if (searchVRDevicesParam.IsPaginated)
        {
            ApplyPaging(searchVRDevicesParam.PageSize * (searchVRDevicesParam.PageIndex - 1), searchVRDevicesParam.PageSize);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchVRDevicesParam.SortBy))
        {
            searchVRDevicesParam.SortBy = "nameAsc";
        }

        switch (searchVRDevicesParam.SortBy)
        {
            case "nameAsc":
                AddOrderBy(x => x.Name);
                break;
            case "nameDesc":
                AddOrderByDescending(x => x.Name);
                break;
            case "createdAtUtcAsc":
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case "createdAtUtcDesc":
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            case "updatedAtUtc":
                AddOrderBy(x => x.UpdatedAtUtc);
                break;
            case "updatedAtUtcDesc":
                AddOrderByDescending(x => x.UpdatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
