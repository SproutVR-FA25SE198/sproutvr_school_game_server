using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.DesignVRLessonPreset;

public sealed class TeacherDesignVRLessonPresetCommandValidator
    : AbstractValidator<TeacherDesignVRLessonPresetCommand>
{
    public TeacherDesignVRLessonPresetCommandValidator()
    {
        RuleFor(x => x.VRLessonId)
        .NotEmpty().WithMessage("VRLessonId is required.");

        RuleFor(x => x.TaskConfigs)
            .NotEmpty().WithMessage("At least one TaskConfig is required.");

        RuleForEach(x => x.TaskConfigs)
            .SetValidator(new DesignVRLessonPresetTaskConfigValidator());
    }
}

public sealed class DesignVRLessonPresetTaskConfigValidator
    : AbstractValidator<DesignVRLessonPresetTaskConfigRequestDto>
{
    public DesignVRLessonPresetTaskConfigValidator()
    {
        RuleFor(x => x.VRTaskId)
            .NotEmpty().WithMessage("VRTaskId is required for each task config.");

        // Must provide answers if provide question
        When(x => !string.IsNullOrEmpty(x.Question), () =>
        {
            RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("Answers must be provided if Question is set.");
        });

        // Must provide question if provide answers
        When(x => x.Answers != null && x.Answers.Any(), () =>
        {
            RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Question must be provided if Answers are set.");
        });

        // An 'info' task cannot be a 'quiz' task
        When(x => !string.IsNullOrEmpty(x.Information), () =>
        {
            RuleFor(x => x.Question)
                .Empty().WithMessage("Question must be empty for an 'info' task.");
            RuleFor(x => x.Answers)
                .Empty().WithMessage("Answers must be empty for an 'info' task.");
        });

        RuleForEach(x => x.Answers)
            .SetValidator(new DesignVRLessonPresetAnswerValidator())
            .When(x => x.Answers != null);
    }
}

public sealed class DesignVRLessonPresetAnswerValidator
    : AbstractValidator<DesignVRLessonPresetAnswerRequestDto>
{
    public DesignVRLessonPresetAnswerValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Answer text is required.");
    }
}
