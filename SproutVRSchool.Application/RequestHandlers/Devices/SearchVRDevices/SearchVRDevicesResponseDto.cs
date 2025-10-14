using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public record SearchVRDevicesResponseDto(
    Guid Id,
    string Name,
    string SerializeNumber,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietnam)
{ }
