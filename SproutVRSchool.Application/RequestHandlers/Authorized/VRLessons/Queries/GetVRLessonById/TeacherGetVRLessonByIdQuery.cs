using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;

public record TeacherGetVRLessonByIdQuery(Guid id)
    : IRequest<TeacherGetVRLessonByIdResponseDto>
{
}
