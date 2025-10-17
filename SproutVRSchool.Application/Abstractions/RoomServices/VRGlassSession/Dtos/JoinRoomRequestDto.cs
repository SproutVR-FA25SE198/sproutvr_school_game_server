namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record JoinRoomRequestDto(
    string RoomCode,
    string VrDeviceSerialNumber,
    string VrDeviceName);
