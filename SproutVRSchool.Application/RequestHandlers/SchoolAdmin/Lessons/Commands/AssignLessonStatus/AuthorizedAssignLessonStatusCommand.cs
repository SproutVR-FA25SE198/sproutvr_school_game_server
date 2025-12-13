using MediatR;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Lessons.Commands.AssignLessonStatus;

public sealed record AuthorizedAssignLessonStatusCommand : IRequest<AuthorizedAssignLessonStatusCommandResponseDto>
{
    public Guid LessonId { get; set; }
    public LessonStatus Status { get; init; }
}

