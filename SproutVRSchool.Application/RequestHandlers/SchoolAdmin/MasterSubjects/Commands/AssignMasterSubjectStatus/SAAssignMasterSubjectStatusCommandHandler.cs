using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.MasterSubjects.Commands.AssignMasterSubjectStatus;

public sealed class SAAssignMasterSubjectStatusCommandHandler(
    IUnitOfWork uow,
    ILogger<SAAssignMasterSubjectStatusCommandHandler> logger
    )
    : IRequestHandler<SAAssignMasterSubjectStatusCommand, SAAssignMasterSubjectStatusCommandResponseDto>
{
    public async Task<SAAssignMasterSubjectStatusCommandResponseDto> Handle(
        SAAssignMasterSubjectStatusCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find existing master subject or throw
        MasterSubject? masterSubject = await uow.Repository<MasterSubject>().GetEntityByIdAsync(
            request.MasterSubjectId
        ) ?? throw new SvrResourceNotFoundException($"Master Subject with ID {request.MasterSubjectId} not found.");

        // 2. Assign status
        masterSubject.UpdateStatus(request.Status);

        // 3. Save changes
        uow.Repository<MasterSubject>().Update(masterSubject);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Master Subject '{MasterSubjectName}' status updated to {Status}", masterSubject.Name, masterSubject.Status);

        return new SAAssignMasterSubjectStatusCommandResponseDto(
            MasterSubjectId: request.MasterSubjectId,
            MasterSubjectName: masterSubject.Name,
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(masterSubject.Status)
        );
    }
}
