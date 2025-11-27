using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Lessons.Commands.AssignLessonStatus;

public sealed record SAAssignLessonStatusCommandResponseDto(
    Guid LessonId,
    string LessonName,
    string Message,
    StatusDto Status
);

