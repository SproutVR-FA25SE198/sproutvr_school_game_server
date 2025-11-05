using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.CreateVRLesson;

public sealed class TeacherCreateVRLessonCommandValidator : AbstractValidator<TeacherCreateVRLessonCommand>
{
    public TeacherCreateVRLessonCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("LessonId is required.");

        RuleFor(x => x.MapId)
            .NotEmpty().WithMessage("MapId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("DeviceName is required.")
            .MaximumLength(100).WithMessage("DeviceName must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.MaxDuration)
            .GreaterThan(TimeSpan.Zero).WithMessage("MaxDuration must be a positive time span.");

        RuleFor(x => x.Tasks)
            .NotEmpty().WithMessage("At least one task is required.");

        RuleForEach(x => x.Tasks)
            .SetValidator(new CreateVRLessonTaskValidator());
    }
}

public sealed class CreateVRLessonTaskValidator : AbstractValidator<CreateVRLessonTaskRequestDto>
{
    public CreateVRLessonTaskValidator()
    {
        RuleFor(x => x.TaskLocationId)
            .NotEmpty().WithMessage("TaskLocationId is required.");

        RuleFor(x => x.MapObjectId)
            .NotEmpty().WithMessage("MapObjectId is required.");

        RuleFor(x => x.ActivityTypeId)
            .NotEmpty().WithMessage("ActivityTypeId is required.");

        RuleFor(x => x.TaskNumber)
            .NotEmpty().WithMessage("TaskNumber is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Task Description is required.")
            .MaximumLength(255).WithMessage("Task Description must not exceed 255 characters.");
    }
}
