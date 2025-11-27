using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.SearchVRDevices;

public sealed class AuthorizedSearchVRDevicesQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedSearchVRDevicesQuery, GetListResultResponseDto<AuthorizedSearchVRDevicesQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchVRDevicesQueryResponseDto>> Handle(AuthorizedSearchVRDevicesQuery request, CancellationToken cancellationToken)
    {
        // Get items & count from params
        (IReadOnlyList<VRDevice> Data, int Count) rawLists = await uow.Repository<VRDevice>()
            .ListAsync(new VRDevicesSpecification(request.AuthorizedSearchVRDevicesParams));

        var result = new GetListResultResponseDto<AuthorizedSearchVRDevicesQueryResponseDto>(
            pageSize: request.AuthorizedSearchVRDevicesParams.PageSize,
            pageIndex: request.AuthorizedSearchVRDevicesParams.PageIndex,
            totalItems: rawLists.Count,
            items: [.. rawLists.Data.Select(device => new AuthorizedSearchVRDevicesQueryResponseDto(
                Id: device.Id,
                Name: device.Name,
                SerializeNumber: device.SerialNumber,
                Status: new StatusDto(device.Status),
                CreatedAtUtc: device.CreatedAtUtc,
                CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(device.CreatedAtUtc))
            )]
        );

        return result;
    }
}
