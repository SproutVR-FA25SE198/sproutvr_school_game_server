using MediatR;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.MasterSubjects.Commands.AssignMasterSubjectStatus;

// ===================== COMMAND =====================
public sealed record SAAssignMasterSubjectStatusCommand : IRequest<SAAssignMasterSubjectStatusCommandResponseDto>
{
    public Guid MasterSubjectId { get; set; }
    public MasterSubjectStatus Status { get; init; }
}
