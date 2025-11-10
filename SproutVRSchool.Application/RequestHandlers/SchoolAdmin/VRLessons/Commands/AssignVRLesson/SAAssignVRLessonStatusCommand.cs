using MediatR;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRLessons.Commands.AssignVRLesson;

public sealed record SAAssignVRLessonStatusCommand : IRequest<SAAssignVRLessonStatusCommandResponseDto>
{
    public Guid VRLessonId { get; set; }
    public VRLessonStatus Status { get; init; }
}

