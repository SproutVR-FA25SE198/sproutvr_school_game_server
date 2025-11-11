using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.GetTeacherAccountById;

public sealed record AuthorizedGetTeacherAccountByIdQuery
    : IRequest<AuthorizedGetTeacherAccountByIdQueryResponseDto>
{
    public Guid TeacherId { get; init; }
}
