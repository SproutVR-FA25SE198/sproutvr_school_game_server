using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;

public record AuthorizedGetVRLessonByIdQuery(Guid Id)
    : IRequest<AuthorizedGetVRLessonByIdQueryResponseDto>
{
}
