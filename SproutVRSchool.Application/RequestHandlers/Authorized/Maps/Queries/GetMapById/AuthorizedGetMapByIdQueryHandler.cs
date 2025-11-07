using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.Maps;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.GetMapById;

public sealed class AuthorizedGetMapByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetMapByIdQuery, AuthorizedGetMapByIdQueryResponseDto>
{
    public async Task<AuthorizedGetMapByIdQueryResponseDto> Handle(AuthorizedGetMapByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the Map entity from the repo, including the Subject
        var spec = new MapsSpecification(request.Id);

        Map? map = await uow.Repository<Map>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (map is null)
        {
            throw new SvrResourceNotFoundException($"Map with ID {request.Id} not found.");
        }

        // 3. Map the Subject info
        var subjectDto = new AuthorizedGetMapByIdQuerySubjectResponseDto
        {
            Id = map.Subject.Id,
            Name = map.Subject.Name,
            ImageUrl = map.Subject.ImageUrl,
        };

        // 4. Return the response DTO
        return new AuthorizedGetMapByIdQueryResponseDto
        {
            Id = map.Id,
            Subject = subjectDto,
            MapCode = map.MapCode,
            Name = map.Name,
            Description = map.Description,
            ImageUrl = map.ImageUrl,
            PreviewUrl = map.PreviewUrl,
            Status = new StatusDto(map.Status),
            CreatedAtUtc = map.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(map.CreatedAtUtc)
        };
    }
}
