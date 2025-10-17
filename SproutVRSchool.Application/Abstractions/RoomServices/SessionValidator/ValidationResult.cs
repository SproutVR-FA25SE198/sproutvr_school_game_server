using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;

/// <summary>
/// This interface will validate the learning session state
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; init; }
    public JoinRoomResponseDto JoinRoomResponseDto { get; init; }

    // after validation, return the learning session info to avoid fetching twice
    public ModelVRLearningSession? VrLearningSession { get; init; }
}
