using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRLessons.Commands.AssignVRLesson;

public sealed record SAAssignVRLessonStatusCommandResponseDto(
    Guid VRLessonId,
    string VRLessonName,
    string Message,
    StatusDto Status
);

