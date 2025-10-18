using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record JoinRoomRequestDto(
    string RoomCode,
    string VrDeviceSerialNumber,
    string VrDeviceName)
{
    public static JoinRoomRequestDto MapFromGrpcRequest(JoinRoomRequest request) =>
        new JoinRoomRequestDto(
            RoomCode: request.RoomCode,
            VrDeviceSerialNumber: request.VrDeviceSerialNumber,
            VrDeviceName: request.VrDeviceName);
}
