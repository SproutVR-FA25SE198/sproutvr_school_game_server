using FluentValidation.Results;
using SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.CreateVRLesson;

namespace SproutVRSchool.Tests.Features.CreateVRLesson;

public class CreateVRLessonTests
{
    private readonly TeacherCreateVRLessonCommandValidator _validator
        = new TeacherCreateVRLessonCommandValidator();

    [Fact]
    public void Validate_LessonIdIsEmpty_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { LessonId = Guid.Empty };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "LessonId");
    }

    [Fact]
    public void Validate_MapIdIsEmpty_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { MapId = Guid.Empty };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MapId");
    }

    [Fact]
    public void Validate_NameIsEmpty_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Name = string.Empty };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_NameTooLong_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Name = new string('A', 101) };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_DescriptionIsEmpty_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Description = string.Empty };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_DescriptionTooLong_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Description = new string('D', 1001) };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_MaxDurationZero_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { MaxDuration = TimeSpan.Zero };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MaxDuration");
    }

    [Fact]
    public void Validate_TasksEmpty_ShouldFail()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Tasks = [] };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Tasks");
    }

    [Fact]
    public void Validate_TaskWithEmptyTaskLocationId_ShouldFail()
    {
        CreateVRLessonTaskRequestDto task = CreateValidTask() with { TaskLocationId = Guid.Empty };
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Tasks = [task] };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_TaskWithEmptyMapObjectId_ShouldFail()
    {
        CreateVRLessonTaskRequestDto task = CreateValidTask() with { MapObjectId = Guid.Empty };
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Tasks = [task] };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_TaskWithEmptyActivityTypeId_ShouldFail()
    {
        CreateVRLessonTaskRequestDto task = CreateValidTask() with { ActivityTypeId = Guid.Empty };
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Tasks = [task] };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_TaskWithEmptyDescription_ShouldFail()
    {
        CreateVRLessonTaskRequestDto task = CreateValidTask() with { Description = string.Empty };
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Tasks = [task] };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_TaskDescriptionTooLong_ShouldFail()
    {
        CreateVRLessonTaskRequestDto task = CreateValidTask() with { Description = new string('X', 256) };
        TeacherCreateVRLessonCommand command = CreateValidCommand() with { Tasks = [task] };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        TeacherCreateVRLessonCommand command = CreateValidCommand();

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    private static TeacherCreateVRLessonCommand CreateValidCommand()
    {
        return new TeacherCreateVRLessonCommand(
            LessonId: Guid.NewGuid(),
            MapId: Guid.NewGuid(),
            Name: "VR Lesson",
            Description: "Valid description",
            MaxDuration: TimeSpan.FromMinutes(10),
            Tasks:
            [
                CreateValidTask()
            ]
        );
    }

    private static CreateVRLessonTaskRequestDto CreateValidTask()
    {
        return new CreateVRLessonTaskRequestDto(
            TaskLocationId: Guid.NewGuid(),
            MapObjectId: Guid.NewGuid(),
            ActivityTypeId: Guid.NewGuid(),
            TaskNumber: 1,
            Question: null,
            Description: "Task description"
        );
    }
}
