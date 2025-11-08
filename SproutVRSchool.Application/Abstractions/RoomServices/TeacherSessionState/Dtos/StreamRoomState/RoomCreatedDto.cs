namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record RoomCreatedDto(
        string TeacherId,
        string VrLessonId,
        string ClassName,
        string Status
    );
