using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.Maps;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Querries.SearchMaps;

public sealed class AuthorizedSearchMapsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<AuthorizedSearchMapsQuery, GetListResultResponseDto<AuthorizedSearchMapsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchMapsQueryResponseDto>> Handle(AuthorizedSearchMapsQuery request, CancellationToken cancellationToken)
    {
        // Get items & count from params
        (IReadOnlyList<Map> Data, int Count) rawLists = await uow.Repository<Map>()
            .ListAsync(new MapsSpecification(request.SearchMapsParams));

        var result = new GetListResultResponseDto<AuthorizedSearchMapsQueryResponseDto>(
            pageSize: request.SearchMapsParams.PageSize,
            pageIndex: request.SearchMapsParams.PageIndex,
            totalItems: rawLists.Count,
            items: [.. rawLists.Data.Select(map => new AuthorizedSearchMapsQueryResponseDto(
                Id: map.Id,
                Name: map.Name,
                Subject: new AuthorizedSearchMapsQuerySubjectResponseDto(
                    Id: map.Subject.Id,
                    Name: map.Subject.Name,
                    Description: map.Subject.Description,
                    ImageUrl: map.Subject.ImageUrl),
                MapCode: map.MapCode,
                Status: new StatusDto(map.Status),
                CreatedAtUtc: map.CreatedAtUtc,
                CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(map.CreatedAtUtc))
            )]
        );

        return result;
    }
}

