using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;

public sealed record SearchVRDevicesQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<SearchVRDevicesQuery, PaginatedResultDto<SearchVRDevicesResponseDto>>
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
            data: [.. rawLists.Data.Select(device => new SearchVRDevicesResponseDto(
                Name: device.Name,
                SerializeNumber: device.SerialNumber,
                Status: new StatusDto((int)device.Status, device.Status.ToString()),
                CreatedAtUtc: device.CreatedAtUtc,
                CreatedAtVietnam: dateTimeProvider.ConvertToVietNamTime(device.CreatedAtUtc))
            )]
        );

        return result;
    }
}
