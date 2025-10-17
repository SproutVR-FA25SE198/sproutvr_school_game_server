using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.CreateVRLesson;

public sealed class CreateVRLessonCommandValidator : AbstractValidator<CreateVRLessonCommand>
{
    public CreateVRLessonCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("LessonId is required.");

        RuleFor(x => x.MapId)
            .NotEmpty().WithMessage("MapId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.MaxDuration)
            .NotEmpty().WithMessage("MaxDuration is required.")
            .GreaterThan(TimeSpan.Zero).WithMessage("MaxDuration must be greater than zero.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .MaximumLength(300).WithMessage("ImageUrl must not exceed 300 characters.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("'{PropertyName}' must be a valid URL.");
    }
}
