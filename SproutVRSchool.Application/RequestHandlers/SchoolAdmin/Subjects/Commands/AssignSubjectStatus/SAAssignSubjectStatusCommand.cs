using MediatR;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Subjects.Commands.AssignSubjectStatus;

public sealed record SAAssignSubjectStatusCommand : IRequest<SAAssignSubjectStatusCommandResponseDto>
{
    public Guid SubjectId { get; set; }
    public SubjectStatus Status { get; init; }
}
