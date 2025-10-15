using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Application.Abstractions.RoomSession;

public record JoinRoomResult(
    JoinStatus Status,
    string Message,
    string RoomId = "",
    string? PresetJsonContent = null);
