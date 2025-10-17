namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record TaskUpdateDto(
    string VrLearningSessionId,
    string VrDeviceSerialNumber,
    string VrTaskId,
    bool IsCompleted,
    bool IsCorrect);
