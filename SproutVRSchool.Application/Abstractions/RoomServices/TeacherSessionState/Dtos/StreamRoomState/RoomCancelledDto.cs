namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record RoomCancelledDto(
    string CancelledBy,
    string Reason
);
