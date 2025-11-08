namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public sealed record RoomCreatedDto(
        string TeacherId,
        string VrLessonId,
        string ClassName,
        string Status
    );
