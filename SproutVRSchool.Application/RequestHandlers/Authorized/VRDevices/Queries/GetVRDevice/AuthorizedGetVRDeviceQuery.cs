using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.GetVRDevice;

public record AuthorizedGetVRDeviceQuery(
    Guid Id) : IRequest<AuthorizedGetVRDeviceQueryResponseDto>;
