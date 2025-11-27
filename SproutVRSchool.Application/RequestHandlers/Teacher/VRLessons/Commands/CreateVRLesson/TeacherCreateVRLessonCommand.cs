using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.CreateVRLesson;

public record TeacherCreateVRLessonCommand(
    Guid LessonId,
    Guid MapId,
    string Name,
    string Description,
    TimeSpan MaxDuration,
    List<CreateVRLessonTaskRequestDto> Tasks) : IRequest<Guid>;

public record CreateVRLessonTaskRequestDto(
    Guid TaskLocationId,
    Guid MapObjectId,
    Guid ActivityTypeId,
    int TaskNumber,
    string? Question,
    string Description
);
