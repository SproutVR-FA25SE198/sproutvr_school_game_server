namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public sealed record RoomEndedDto(
    string EndedBy,
    string Reason,
    DateTime EndTimeAtUtc
);
