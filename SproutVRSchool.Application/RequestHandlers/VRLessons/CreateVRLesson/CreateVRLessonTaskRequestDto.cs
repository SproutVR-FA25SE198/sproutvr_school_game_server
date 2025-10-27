namespace SproutVRSchool.Application.RequestHandlers.VRLessons.CreateVRLesson;

public record CreateVRLessonTaskRequestDto(
    Guid TaskLocationId,
    Guid MapObjectId,
    Guid ActivityTypeId,
    int TaskNumber,
    string Description
);
