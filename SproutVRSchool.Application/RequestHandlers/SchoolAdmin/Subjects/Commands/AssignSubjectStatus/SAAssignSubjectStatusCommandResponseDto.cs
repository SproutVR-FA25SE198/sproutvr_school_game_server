using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Subjects.Commands.AssignSubjectStatus;

public sealed record SAAssignSubjectStatusCommandResponseDto(
    Guid SubjectId,
    string SubjectName,
    string Message,
    StatusDto Status
);
