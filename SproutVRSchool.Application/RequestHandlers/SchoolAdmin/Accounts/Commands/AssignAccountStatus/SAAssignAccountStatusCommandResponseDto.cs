using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.AssignAccountStatus;

public sealed record SAAssignAccountStatusCommandResponseDto(
    Guid UserId,
    string AccountName,
    string Message,
    StatusDto Status
);
