using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRLessons.Commands.AssignVRLesson;

public sealed class SAAssignVRLessonStatusCommandValidator : AbstractValidator<SAAssignVRLessonStatusCommand>
{
    public SAAssignVRLessonStatusCommandValidator()
    {
        RuleFor(x => x.VRLessonId)
            .NotEmpty().WithMessage("VRLessonId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid VR lesson status. Values are 0: Inactive | 1: Active");
    }
}

