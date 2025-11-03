using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Queries.GetLessonById;

public record TeacherGetLessonByIdQuery(Guid Id)
    : IRequest<TeacherGetLessonByIdQueryResponseDto>
{
}
