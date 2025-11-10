using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Subjects.Commands.AssignSubjectStatus;

public sealed class SAAssignSubjectStatusCommandHandler(
    IUnitOfWork uow,
    ILogger<SAAssignSubjectStatusCommandHandler> logger
    )
    : IRequestHandler<SAAssignSubjectStatusCommand, SAAssignSubjectStatusCommandResponseDto>
{
    public async Task<SAAssignSubjectStatusCommandResponseDto> Handle(
        SAAssignSubjectStatusCommand request,
        CancellationToken cancellationToken)
    {
        // 1. If existing subject, then throw error
        Subject? subject = await uow.Repository<Subject>().GetEntityByIdAsync(
            request.SubjectId
        ) ?? throw new SvrResourceNotFoundException($"Subject with ID {request.SubjectId} not found.");

        // 2. Assign status
        subject.UpdateStatus(request.Status);

        // 3. Save changes
        uow.Repository<Subject>().Update(subject);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Subject '{SubjectName}' status updated to {Status}", subject.Name, subject.Status);

        return new SAAssignSubjectStatusCommandResponseDto(
            SubjectId: request.SubjectId,
            SubjectName: subject.Name,
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(subject.Status)
        );
    }
}
