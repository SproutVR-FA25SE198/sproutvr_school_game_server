using MediatR;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.AssignVRDeviceStatus;

public sealed record SAAssignVRDeviceStatusCommand : IRequest<SAAssignVRDeviceStatusCommandResponseDto>
{
    public Guid DeviceId { get; set; }
    public VRDeviceStatus Status { get; init; }
}


