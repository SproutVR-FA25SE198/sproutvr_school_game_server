using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.GetMapById;

public record AuthorizedGetMapByIdQuery(Guid Id)
    : IRequest<AuthorizedGetMapByIdQueryResponseDto>
{
}
