using MediatR;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Commands.AssignVRLesson;

public sealed record AuthorizedAssignVRLessonStatusCommand : IRequest<AuthorizedAssignVRLessonStatusCommandResponseDto>
{
    public Guid VRLessonId { get; set; }
    public VRLessonStatus Status { get; init; }
}

