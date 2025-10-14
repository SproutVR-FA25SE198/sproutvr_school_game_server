using MediatR;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public sealed record SearchVRDevicesQuery(SearchVRDevicesParams SearchVRDevicesParams)
    : IRequest<PaginatedResultDto<SearchVRDevicesResponseDto>>
{
}
