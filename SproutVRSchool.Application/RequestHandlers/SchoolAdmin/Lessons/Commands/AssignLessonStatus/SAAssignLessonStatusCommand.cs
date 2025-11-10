using MediatR;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Lessons.Commands.AssignLessonStatus;

public sealed record SAAssignLessonStatusCommand : IRequest<SAAssignLessonStatusCommandResponseDto>
{
    public Guid LessonId { get; set; }
    public LessonStatus Status { get; init; }
}

