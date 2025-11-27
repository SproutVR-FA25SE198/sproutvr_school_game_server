using LearningSession.V1;
using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record JoinRoomResponseDto
{
    // ============================
    // === Properties
    // ============================

    public JoinStatus Status { get; init; }
    public string Message { get; init; }

    // allow null when join failed
    public string? VrLearningSessionId { get; init; }

    // allow null when join failed, and can be set later after stringify
    public string? PresetJsonContent { get; set; }

    // ============================
    // === Constructor
    // ============================

    public JoinRoomResponseDto(JoinStatus status, string message, string? vrLearningSessionId = null, string? presetJsonContent = null)
    {
        Status = status;
        Message = message;
        VrLearningSessionId = vrLearningSessionId;
        PresetJsonContent = presetJsonContent;
    }

    // ============================
    // === Methods
    // ============================

    public static JoinRoomResponse MapToGrpcResponse(JoinRoomResponseDto joinRoomResponseDto) =>
        new JoinRoomResponse
        {
            Status = joinRoomResponseDto.Status,
            Message = joinRoomResponseDto.Message,
            VrLearningSessionId = joinRoomResponseDto.VrLearningSessionId ?? string.Empty,
            PresetJsonContent = joinRoomResponseDto.PresetJsonContent ?? string.Empty,
        };
}


