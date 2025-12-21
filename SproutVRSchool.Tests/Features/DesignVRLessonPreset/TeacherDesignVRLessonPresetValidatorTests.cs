using FluentValidation.Results;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.DesignVRLessonPreset;
using SproutVRSchool.Infrastructure.Data;

namespace SproutVRSchool.Tests.Features.DesignVRLessonPreset;

public class TeacherDesignVRLessonPresetValidatorTests
{
    private readonly TeacherDesignVRLessonPresetCommandValidator _validator = new();

    public static SchoolServerDbContext CreateNoDataInMemory()
    {
        DbContextOptions<SchoolServerDbContext> options = new DbContextOptionsBuilder<SchoolServerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        return new SchoolServerDbContext(options);
    }

    private static TeacherDesignVRLessonPresetCommand CreateValidCommand()
    {
        return new TeacherDesignVRLessonPresetCommand(
            IsSequential: true,
            TaskConfigs:
            [
                new DesignVRLessonPresetTaskConfigRequestDto(
                    VRTaskId: Guid.NewGuid(),
                    Question: "What is VR?",
                    Answers:
                    [
                        new DesignVRLessonPresetAnswerRequestDto("Virtual Reality", true)
                    ],
                    Information: null
                )
            ]
        )
        {
            VRLessonId = Guid.NewGuid()
        };
    }

    [Fact]
    public void Validate_VRLessonIdIsEmpty_ShouldFail()
    {
        TeacherDesignVRLessonPresetCommand command = CreateValidCommand();
        command.VRLessonId = Guid.Empty;

        ValidationResult result = _validator.TestValidate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "VRLessonId");
    }

    [Fact]
    public void DesignVRLessonPresetValidator_ValidCommand_ShouldPass()
    {
        TeacherDesignVRLessonPresetCommand command = CreateValidCommand();
        ValidationResult result = _validator.TestValidate(command);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void DesignVRLessonPresetValidato_AnswerTextIsEmpty_ShouldFail()
    {
        var command = new TeacherDesignVRLessonPresetCommand(
            IsSequential: false,
            TaskConfigs:
            [
                new DesignVRLessonPresetTaskConfigRequestDto(
                    VRTaskId: Guid.NewGuid(),
                    Question: "What is VR?",
                    Answers:
                    [
                        new DesignVRLessonPresetAnswerRequestDto("", true)
                    ],
                    Information: null
                )
            ]
        )
        {
            VRLessonId = Guid.NewGuid()
        };

        ValidationResult result = _validator.TestValidate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Text"));
    }

    [Fact]
    public void DesignVRLessonPresetValidato_InfoTaskWithAnswers_ShouldFail()
    {
        var command = new TeacherDesignVRLessonPresetCommand(
            IsSequential: true,
            TaskConfigs:
            [
                new DesignVRLessonPresetTaskConfigRequestDto(
                    VRTaskId: Guid.NewGuid(),
                    Question: null,
                    Answers:
                    [
                        new DesignVRLessonPresetAnswerRequestDto("Wrong", false)
                    ],
                    Information: "Some info"
                )
            ]
        )
        {
            VRLessonId = Guid.NewGuid()
        };

        ValidationResult result = _validator.TestValidate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Answers"));
    }

    [Fact]
    public void DesignVRLessonPresetValidato_InfoTaskWithQuestion_ShouldFail()
    {
        var command = new TeacherDesignVRLessonPresetCommand(
            IsSequential: true,
            TaskConfigs:
            [
                new DesignVRLessonPresetTaskConfigRequestDto(
                    VRTaskId: Guid.NewGuid(),
                    Question: "Invalid",
                    Answers: null,
                    Information: "Some info"
                )
            ]
        )
        {
            VRLessonId = Guid.NewGuid()
        };

        ValidationResult result = _validator.TestValidate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Question"));
    }
}
