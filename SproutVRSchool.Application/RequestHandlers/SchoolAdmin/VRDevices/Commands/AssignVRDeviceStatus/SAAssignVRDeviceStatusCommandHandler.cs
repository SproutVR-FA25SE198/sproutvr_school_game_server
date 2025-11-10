using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.AssignVRDeviceStatus;

public sealed class SAAssignVRDeviceStatusCommandHandler(
    IUnitOfWork uow,
    ILogger<SAAssignVRDeviceStatusCommandHandler> logger
    )
    : IRequestHandler<SAAssignVRDeviceStatusCommand, SAAssignVRDeviceStatusCommandResponseDto>
{
    public async Task<SAAssignVRDeviceStatusCommandResponseDto> Handle(
        SAAssignVRDeviceStatusCommand request,
        CancellationToken cancellationToken)
    {
        // 1. If existing device, then throw error
        VRDevice? vrDevice = await uow.Repository<VRDevice>().GetEntityByIdAsync(
            request.DeviceId
        ) ?? throw new SvrResourceNotFoundException($"VR Device with ID {request.DeviceId} not found.");

        // 2. Assign status
        vrDevice.UpdateStatus(request.Status);

        // 3. Save changes
        uow.Repository<VRDevice>().Update(vrDevice);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("VR Device '{DeviceName}' status updated to {Status}", vrDevice.Name, vrDevice.Status);

        return new SAAssignVRDeviceStatusCommandResponseDto(
            DeviceId: request.DeviceId,
            DeviceName: "Dummy Device",
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(vrDevice.Status)
        );
    }
}
