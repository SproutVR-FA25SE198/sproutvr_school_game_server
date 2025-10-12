using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public sealed record SearchVRDevicesQuery(SearchVRDevicesParams SearchVRDevicesParams)
    : IRequest<PaginatedResultDto<SearchVRDevicesResponseDto>>
{
}
