namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record RoomActivatedDto(
    string RoomCode,
    DateTime StartTimeAtUtc,
    int DurationInSeconds
);
