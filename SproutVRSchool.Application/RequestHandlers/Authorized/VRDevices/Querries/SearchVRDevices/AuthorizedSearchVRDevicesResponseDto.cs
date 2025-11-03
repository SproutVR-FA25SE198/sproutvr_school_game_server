using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Querries.SearchVRDevices;

public record AuthorizedSearchVRDevicesResponseDto(
    Guid Id,
    string Name,
    string SerializeNumber,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam)
{ }
