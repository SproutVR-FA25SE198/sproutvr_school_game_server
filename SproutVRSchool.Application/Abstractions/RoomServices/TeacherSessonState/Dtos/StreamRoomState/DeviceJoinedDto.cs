namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessonState.Dtos.StreamRoomState;

public sealed record DeviceJoinedDto(
    string VrDeviceSerialNumber,
    string StudentName,
    string ConnectionStatus
);
