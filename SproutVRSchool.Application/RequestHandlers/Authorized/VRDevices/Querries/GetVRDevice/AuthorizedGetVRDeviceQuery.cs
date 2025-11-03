using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Querries.GetVRDevice;

public record AuthorizedGetVRDeviceQuery(
    Guid Id) : IRequest<AuthorizedGetVRDeviceQueryResponseDto>;
