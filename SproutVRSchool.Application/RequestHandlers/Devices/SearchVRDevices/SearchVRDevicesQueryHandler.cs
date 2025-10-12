using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public sealed record SearchVRDevicesQueryHandler(IUnitOfWork uow) : IRequestHandler<SearchVRDevicesQuery, PaginatedResultDto<SearchVRDevicesResponseDto>>
{
    public async Task<PaginatedResultDto<SearchVRDevicesResponseDto>> Handle(SearchVRDevicesQuery request, CancellationToken cancellationToken)
    {
        // Get data & count from params
        (IReadOnlyList<VRDevice> Data, int Count) rawLists = await uow.Repository<VRDevice>()
            .ListAsync(new VRDevicesSpecification(request.SearchVRDevicesParams));

        var result = new PaginatedResultDto<SearchVRDevicesResponseDto>(
            pageSize: request.SearchVRDevicesParams.PageSize,
            pageIndex: request.SearchVRDevicesParams.PageIndex,
            count: rawLists.Count,
            data: [.. rawLists.Data.Select(device => SearchVRDevicesResponseDto.FromEntity(device))]
        );

        return result;
    }
}
