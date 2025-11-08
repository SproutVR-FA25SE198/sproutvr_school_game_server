namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record RoomEndedDto(
    string EndedBy,
    string Reason,
    DateTime EndTimeAtUtc
);
