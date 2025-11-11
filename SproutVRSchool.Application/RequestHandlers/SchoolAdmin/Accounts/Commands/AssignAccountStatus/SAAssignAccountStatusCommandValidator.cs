using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.AssignAccountStatus;

public sealed class SAAssignAccountStatusCommandValidator : AbstractValidator<SAAssignAccountStatusCommand>
{
    public SAAssignAccountStatusCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid account status. Values are 0: Active | 1: Disabled");
    }
}
