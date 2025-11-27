using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.GetLessonById;

public record AuthorizedGetLessonByIdQuery(Guid Id)
    : IRequest<AuthorizedGetLessonByIdQueryResponseDto>
{
}
