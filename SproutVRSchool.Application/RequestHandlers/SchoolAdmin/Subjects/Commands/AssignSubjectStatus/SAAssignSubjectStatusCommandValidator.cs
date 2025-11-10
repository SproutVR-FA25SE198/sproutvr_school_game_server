using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Subjects.Commands.AssignSubjectStatus;

public sealed class SAAssignSubjectStatusCommandValidator : AbstractValidator<SAAssignSubjectStatusCommand>
{
    public SAAssignSubjectStatusCommandValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("SubjectId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid subject status. Values are 0: Inactive | 1: Active");
    }
}
