using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.UpdateLesson;

public sealed class TeacherUpdateLessonCommandValidator : AbstractValidator<TeacherUpdateLessonCommand>
{
    // ============================
    // === Fields
    // ============================

    // 10 MB
    private const long MaxFileSizeInBytes = 10 * 1024 * 1024;

    private static readonly string[] AllowedExtensions = { ".pdf" };

    // ============================
    // === Constructors
    // ============================
    public TeacherUpdateLessonCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("Lesson ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("DeviceName cannot be set to an empty value.")
            .MaximumLength(100).WithMessage("DeviceName must not exceed 100 characters.")
            .When(x => x.Name != null); // Apply these rules ONLY if DeviceName is not null.

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("TaskDescription cannot be set to an empty value.")
            .MaximumLength(1000).WithMessage("TaskDescription must not exceed 1000 characters.")
            .When(x => x.Description != null); // Apply these rules ONLY if TaskDescription is not null.

        When(x => x.ResourceFile != null, () =>
        {
            // rule for file size
            RuleFor(x => x.ResourceFile!.Length)
                .LessThanOrEqualTo(MaxFileSizeInBytes)
                .WithMessage($"File size must not exceed {MaxFileSizeInBytes / 1024 / 1024}MB.");

            RuleFor(x => x.ResourceFile!.FileName)
                .Must(IsValidExtension)
                .WithMessage($"File type is not supported. Allowed extensions are: {string.Join(", ", AllowedExtensions)}");
        });
    }

    // ============================
    // === Methods
    // ============================

    private static bool IsValidExtension(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        return !string.IsNullOrEmpty(extension) && AllowedExtensions.Contains(extension);
    }
}
