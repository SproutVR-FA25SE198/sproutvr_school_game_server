using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRLessons.Commands.AssignVRLesson;

public sealed class SAAssignVRLessonStatusCommandHandler(
    IUnitOfWork uow,
    ILogger<SAAssignVRLessonStatusCommandHandler> logger
    )
    : IRequestHandler<SAAssignVRLessonStatusCommand, SAAssignVRLessonStatusCommandResponseDto>
{
    public async Task<SAAssignVRLessonStatusCommandResponseDto> Handle(
        SAAssignVRLessonStatusCommand request,
        CancellationToken cancellationToken)
    {
        VRLesson? vrLesson = await uow.Repository<VRLesson>().GetEntityByIdAsync(
            request.VRLessonId
        ) ?? throw new SvrResourceNotFoundException($"VR Lesson with ID {request.VRLessonId} not found.");

        vrLesson.UpdateStatus(request.Status);

        uow.Repository<VRLesson>().Update(vrLesson);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("VR Lesson '{VRLessonName}' status updated to {Status}", vrLesson.Name, vrLesson.Status);

        return new SAAssignVRLessonStatusCommandResponseDto(
            VRLessonId: request.VRLessonId,
            VRLessonName: vrLesson.Name,
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(vrLesson.Status)
        );
    }
}

