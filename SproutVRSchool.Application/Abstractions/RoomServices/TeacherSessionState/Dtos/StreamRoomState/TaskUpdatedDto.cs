namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record TaskUpdatedDto(
    string VrDeviceSerialNumber,
    string VrTaskId,
    bool IsCompleted,
    bool IsCorrect,
    DateTime UpdatedAtUtc
);
