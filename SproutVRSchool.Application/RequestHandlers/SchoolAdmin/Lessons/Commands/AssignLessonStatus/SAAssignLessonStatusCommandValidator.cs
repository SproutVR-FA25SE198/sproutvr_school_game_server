using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Lessons.Commands.AssignLessonStatus;

public sealed class SAAssignLessonStatusCommandValidator : AbstractValidator<SAAssignLessonStatusCommand>
{
    public SAAssignLessonStatusCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("LessonId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid lesson status. Values are 0: Inactive | 1: Active");
    }
}

