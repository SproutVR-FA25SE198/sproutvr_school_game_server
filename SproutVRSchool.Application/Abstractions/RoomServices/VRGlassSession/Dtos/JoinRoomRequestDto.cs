using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record JoinRoomRequestDto(
    string RoomCode,
    string VrDeviceSerialNumber)
{
    public static JoinRoomRequestDto MapFromGrpcRequest(JoinRoomRequest request) =>
        new JoinRoomRequestDto(
            RoomCode: request.RoomCode,
            VrDeviceSerialNumber: request.VrDeviceSerialNumber);
}
