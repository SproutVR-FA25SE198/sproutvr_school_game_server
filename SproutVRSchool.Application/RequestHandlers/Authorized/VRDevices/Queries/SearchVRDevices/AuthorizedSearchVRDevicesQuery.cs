using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.SearchVRDevices;

public sealed record AuthorizedSearchVRDevicesQuery(AuthorizedSearchVRDevicesQueryParams AuthorizedSearchVRDevicesParams)
    : IRequest<GetListResultResponseDto<AuthorizedSearchVRDevicesQueryResponseDto>>
{
}
