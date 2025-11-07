using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.GetTaskLocationById;

public sealed class AuthorizedGetTaskLocationByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetTaskLocationByIdQuery, AuthorizedGetTaskLocationByIdQueryResponseDto>
{
    public async Task<AuthorizedGetTaskLocationByIdQueryResponseDto> Handle(AuthorizedGetTaskLocationByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the TaskLocation entity from the repo, including the Map
        var spec = new TaskLocationsSpecification(request.Id);

        TaskLocation? taskLocation = await uow.Repository<TaskLocation>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (taskLocation is null)
        {
            throw new SvrResourceNotFoundException($"TaskLocation with ID {request.Id} not found.");
        }

        // 3. Map the nested Map info
        var mapDto = new AuthorizedGetTaskLocationByIdMapResponseDto
        {
            Id = taskLocation.Map.Id,
            MapCode = taskLocation.Map.MapCode,
            Name = taskLocation.Map.Name,
        };

        // 4. Return the response DTO
        return new AuthorizedGetTaskLocationByIdQueryResponseDto
        {
            Id = taskLocation.Id,
            Map = mapDto,
            LocationCode = taskLocation.LocationCode,
            Name = taskLocation.Name,
            ImageUrl = taskLocation.ImageUrl,
            CreatedAtUtc = taskLocation.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(taskLocation.CreatedAtUtc)
        };
    }
}
