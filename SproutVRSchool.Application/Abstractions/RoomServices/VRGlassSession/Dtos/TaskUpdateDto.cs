namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record TaskUpdateDto(
    string TaskId,
    bool IsCompleted = false,
    bool? IsCorrect = null);
