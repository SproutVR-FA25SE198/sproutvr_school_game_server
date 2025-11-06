using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;

public record AuthorizedGetVRLessonByIdQuery(Guid id)
    : IRequest<AuthorizedGetVRLessonByIdResponseDto>
{
}
