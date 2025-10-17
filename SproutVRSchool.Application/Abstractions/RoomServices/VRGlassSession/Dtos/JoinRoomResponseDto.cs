using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record JoinRoomResponseDto(
    JoinStatus Status,
    string Message,
    string? VrLearningSessionId = null,
    string? PresetJsonContent = null);
