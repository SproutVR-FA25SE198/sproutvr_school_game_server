using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.MapObjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.SearchMapObjects;

public class AuthorizedSearchMapObjectsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchMapObjectsQuery, GetListResultResponseDto<AuthorizedSearchMapObjectsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchMapObjectsQueryResponseDto>> Handle(
        AuthorizedSearchMapObjectsQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new MapObjectsSpecification(request.AuthorizedSearchMapObjectsQueryParams);

        (IReadOnlyList<MapObject> Data, int Count) rawLists =
            await uow.Repository<MapObject>().ListAsync(spec);

        var items = rawLists.Data.Select(mapObject => new AuthorizedSearchMapObjectsQueryResponseDto(
            Id: mapObject.Id,
            ObjectCode: mapObject.ObjectCode,
            Name: mapObject.Name,
            ImageUrl: mapObject.ImageUrl,
            Map: new AuthorizedSearchMapObjectsMapQueryResponseDto
            (
                Id: mapObject.Map.Id,
                MapCode: mapObject.Map.MapCode,
                Name: mapObject.Map.Name,
                ImageUrl: mapObject.Map.ImageUrl,
                PreviewUrl: mapObject.Map.PreviewUrl
            ),
            CreatedAtUtc: mapObject.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(mapObject.CreatedAtUtc)
        )).ToList();

        return new GetListResultResponseDto<AuthorizedSearchMapObjectsQueryResponseDto>(
            pageSize: request.AuthorizedSearchMapObjectsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchMapObjectsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );
    }
}

