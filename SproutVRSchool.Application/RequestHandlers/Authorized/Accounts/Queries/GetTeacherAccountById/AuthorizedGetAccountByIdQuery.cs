using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.GetTeacherAccountById;

public sealed record AuthorizedGetAccountByIdQuery
    : IRequest<AuthorizedGetAccountByIdQueryResponseDto>
{
    public Guid Id { get; init; }
}
