using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public static class SearchVRDevicesCts
{
    // Default sorting
    public const string DefaultSort = NameAsc;

    // Sorting fields
    public const string NameAsc = "nameAsc";
    public const string NameDesc = "nameDesc";
    public const string CreatedAtUtcAsc = "createdAtUtcAsc";
    public const string CreatedAtUtcDesc = "createdAtUtcDesc";
    public const string UpdatedAtUtcAsc = "updatedAtUtcAsc";
    public const string UpdatedAtUtcDesc = "updatedAtUtcDesc";
}
