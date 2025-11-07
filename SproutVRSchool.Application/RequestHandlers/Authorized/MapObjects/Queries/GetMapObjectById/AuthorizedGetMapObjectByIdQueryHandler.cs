using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.MapObjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.GetMapObjectById;

public sealed class AuthorizedGetMapObjectByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetMapObjectByIdQuery, AuthorizedGetMapObjectByIdQueryResponseDto>
{
    public async Task<AuthorizedGetMapObjectByIdQueryResponseDto> Handle(AuthorizedGetMapObjectByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the MapObject entity from the repo, including the Map
        var spec = new MapObjectsSpecification(request.Id);

        MapObject? mapObject = await uow.Repository<MapObject>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (mapObject is null)
        {
            throw new SvrResourceNotFoundException($"MapObject with ID {request.Id} not found.");
        }

        // 3. Map the nested Map info
        var mapDto = new AuthorizedGetMapObjectByIdMapResponseDto
        {
            Id = mapObject.Map.Id,
            MapCode = mapObject.Map.MapCode,
            Name = mapObject.Map.Name,
        };

        // 4. Return the response DTO
        return new AuthorizedGetMapObjectByIdQueryResponseDto
        {
            Id = mapObject.Id,
            Map = mapDto,
            ObjectCode = mapObject.ObjectCode,
            Name = mapObject.Name,
            ImageUrl = mapObject.ImageUrl,
            CreatedAtUtc = mapObject.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(mapObject.CreatedAtUtc)
        };
    }
}
