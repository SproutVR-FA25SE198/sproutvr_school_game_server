using FluentValidation;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.CreateLesson;

public sealed class TeacherCreateLessonCommandValidator : AbstractValidator<TeacherCreateLessonCommand>
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

    public TeacherCreateLessonCommandValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty()
            .WithMessage("Subject ID is required.");

        RuleFor(x => x.TeacherId)
            .NotEmpty().WithMessage("Teacher ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Lesson DeviceName is required.")
            .MaximumLength(100).WithMessage("Lesson DeviceName must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

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
