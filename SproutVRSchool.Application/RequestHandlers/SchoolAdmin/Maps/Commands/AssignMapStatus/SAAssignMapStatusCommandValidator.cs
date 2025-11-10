using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.AssignMapStatus;

public sealed class SAAssignMapStatusCommandValidator : AbstractValidator<SAAssignMapStatusCommand>
{
    public SAAssignMapStatusCommandValidator()
    {
        RuleFor(x => x.MapId)
            .NotEmpty().WithMessage("MapId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid map status. Values are 0: Inactive | 1: Active");
    }
}
