using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Commands.AssignVRLesson;

public sealed class AuthorizedAssignVRLessonStatusCommandValidator : AbstractValidator<AuthorizedAssignVRLessonStatusCommand>
{
    public AuthorizedAssignVRLessonStatusCommandValidator()
    {
        RuleFor(x => x.VRLessonId)
            .NotEmpty().WithMessage("VRLessonId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid VR lesson status. Values are 0: Inactive | 1: Active");
    }
}

