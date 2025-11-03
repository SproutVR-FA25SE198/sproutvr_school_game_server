using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Queries.GetVRLessonById;

public record TeacherGetVRLessonByIdQuery(Guid id)
    : IRequest<TeacherGetVRLessonByIdResponseDto>
{
}
