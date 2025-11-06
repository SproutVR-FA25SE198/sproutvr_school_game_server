namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.AssignVRDeviceStatus;

public sealed record SAAssignVRDeviceStatusCommandResponseDto(
    Guid DeviceId,
    string DeviceName,
    string Message
);
