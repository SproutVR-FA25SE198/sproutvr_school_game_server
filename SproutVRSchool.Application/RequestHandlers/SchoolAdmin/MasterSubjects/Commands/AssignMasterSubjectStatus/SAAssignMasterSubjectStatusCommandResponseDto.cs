using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.MasterSubjects.Commands.AssignMasterSubjectStatus;

// ===================== RESPONSE DTO =====================
public sealed record SAAssignMasterSubjectStatusCommandResponseDto(
    Guid MasterSubjectId,
    string MasterSubjectName,
    string Message,
    StatusDto Status
);
