namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public sealed record RoomCancelledDto(
    string CancelledBy,
    string Reason
);
