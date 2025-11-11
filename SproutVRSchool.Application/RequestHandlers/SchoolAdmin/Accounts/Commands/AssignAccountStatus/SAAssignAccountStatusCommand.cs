using MediatR;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.AssignAccountStatus;

public sealed record SAAssignAccountStatusCommand : IRequest<SAAssignAccountStatusCommandResponseDto>
{
    public Guid UserId { get; set; }
    public UserAccountStatus Status { get; init; }
}
