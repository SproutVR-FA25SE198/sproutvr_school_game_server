namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public sealed record RoomActivatedDto(
    string RoomCode,
    DateTime StartTimeAtUtc,
    int DurationInSeconds
);
