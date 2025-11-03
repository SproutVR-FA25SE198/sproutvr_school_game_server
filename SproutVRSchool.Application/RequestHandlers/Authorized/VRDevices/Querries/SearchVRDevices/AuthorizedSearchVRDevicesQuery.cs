using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Querries.SearchVRDevices;

public sealed record AuthorizedSearchVRDevicesQuery(AuthorizedSearchVRDevicesParams AuthorizedSearchVRDevicesParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchVRDevicesResponseDto>>
{
}
