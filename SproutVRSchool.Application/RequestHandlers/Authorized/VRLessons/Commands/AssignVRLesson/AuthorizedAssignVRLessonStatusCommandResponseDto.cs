using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Commands.AssignVRLesson;

public sealed record AuthorizedAssignVRLessonStatusCommandResponseDto(
    Guid VRLessonId,
    string VRLessonName,
    string Message,
    StatusDto Status
);

