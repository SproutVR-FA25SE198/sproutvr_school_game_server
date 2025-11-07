using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.GetActivityTypeById;

public sealed record AuthorizedGetActivityTypeByIdQuery(Guid Id)
    : IRequest<AuthorizedGetActivityTypeByIdQueryResponseDto>
{
}
