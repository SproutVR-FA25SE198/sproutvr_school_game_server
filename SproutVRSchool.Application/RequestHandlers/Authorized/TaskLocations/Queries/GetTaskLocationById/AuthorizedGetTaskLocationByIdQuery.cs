using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.GetTaskLocationById;

public record AuthorizedGetTaskLocationByIdQuery(Guid Id)
    : IRequest<AuthorizedGetTaskLocationByIdQueryResponseDto>
{
}
