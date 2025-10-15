namespace SproutVRSchool.Application.Abstractions.Services.VRGlassSession.Dtos;

public record JoinRoomRequestDto(
    string RoomCode,
    string DeviceSerialNumber,
    string DeviceName);
