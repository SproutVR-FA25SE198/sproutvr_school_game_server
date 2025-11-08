namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record DeviceDisconnectedDto(
    string VrDeviceSerialNumber,
    string StudentName,
    string ConnectionStatus
);
