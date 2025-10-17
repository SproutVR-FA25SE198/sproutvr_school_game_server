namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record PublishEventRequestDto(
    string VrLearningSessionId,
    string DeviceSerialNumber,
    TaskUpdateDto TaskUpdateDto);
