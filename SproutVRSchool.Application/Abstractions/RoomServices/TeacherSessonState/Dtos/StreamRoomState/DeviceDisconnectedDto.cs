namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public sealed record DeviceDisconnectedDto(
    string VrDeviceSerialNumber,
    string StudentName,
    string ConnectionStatus
);
