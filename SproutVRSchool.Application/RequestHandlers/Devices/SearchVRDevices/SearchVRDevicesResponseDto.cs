using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public record SearchVRDevicesResponseDto(
    Guid Id,
    string Name,
    string SerializeNumber,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietnam)
{
    public static SearchVRDevicesResponseDto FromEntity(VRDevice entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.SerialNumber,
            new StatusDto((int)entity.Status, entity.Status.ToString()),
            entity.CreatedAtUtc,
            entity.CreatedAtUtc.ToOffset(TimeSpan.FromHours(AppCts.TimeOffSet.VN))
            );
}
