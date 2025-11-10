using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.MasterSubjects.Commands.AssignMasterSubjectStatus;

// ===================== VALIDATOR =====================
public sealed class SAAssignMasterSubjectStatusCommandValidator : AbstractValidator<SAAssignMasterSubjectStatusCommand>
{
    public SAAssignMasterSubjectStatusCommandValidator()
    {
        RuleFor(x => x.MasterSubjectId)
            .NotEmpty().WithMessage("MasterSubjectId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid master subject status. Values are 0: Inactive | 1: Active");
    }
}
