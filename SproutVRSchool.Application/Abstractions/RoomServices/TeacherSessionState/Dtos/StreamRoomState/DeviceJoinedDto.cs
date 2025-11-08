namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record DeviceJoinedDto(
    string VrDeviceSerialNumber,
    string StudentName,
    string ConnectionStatus
);
