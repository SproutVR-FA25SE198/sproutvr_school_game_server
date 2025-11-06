using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.AssignVRDeviceStatus;

public sealed class SAAssignVRDeviceStatusCommandValidator : AbstractValidator<SAAssignVRDeviceStatusCommand>
{
    public SAAssignVRDeviceStatusCommandValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("DeviceId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid VR device status. Values are 0: Available | 1: InUse | 2: UnderMaintenance ");
    }
}
