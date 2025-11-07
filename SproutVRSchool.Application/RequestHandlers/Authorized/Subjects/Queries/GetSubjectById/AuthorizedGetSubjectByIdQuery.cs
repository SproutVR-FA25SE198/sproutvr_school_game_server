using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.GetSubjectById;

public record AuthorizedGetSubjectByIdQuery(Guid Id)
    : IRequest<AuthorizedGetSubjectByIdQueryResponseDto>
{
}
