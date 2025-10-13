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
    string Name,
    string SerializeNumber,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietnam)
{ }
