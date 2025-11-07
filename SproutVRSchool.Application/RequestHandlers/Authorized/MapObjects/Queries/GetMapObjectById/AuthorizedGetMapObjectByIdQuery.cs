using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.GetMapObjectById;

public record AuthorizedGetMapObjectByIdQuery(Guid Id)
    : IRequest<AuthorizedGetMapObjectByIdQueryResponseDto>
{
}
