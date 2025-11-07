using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.GetVRTaskById;

public record AuthorizedGetVRTaskByIdQuery(Guid Id)
    : IRequest<AuthorizedGetVRTaskByIdQueryResponseDto>
{
}
