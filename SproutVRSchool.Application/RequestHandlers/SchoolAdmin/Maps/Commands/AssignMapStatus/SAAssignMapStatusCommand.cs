using MediatR;
using SproutVRSchool.Domain.Entities.Maps;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.AssignMapStatus;

public sealed record SAAssignMapStatusCommand : IRequest<SAAssignMapStatusCommandResponseDto>
{
    public Guid MapId { get; set; }
    public MapStatus Status { get; init; }
}
