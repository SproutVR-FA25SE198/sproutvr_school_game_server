using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain.Models.VRLearningSession;
using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;

/// <summary>
/// This interface will validate the learning session state
/// </summary>
public record ValidationResult(
    bool IsValid,
    JoinRoomResponseDto? JoinRoomResponseDto = null,
    ModelVRLearningSession? ModelVRLearningSession = null  // after validation, return the learning session info to avoid fetching twice
)
{
    // VR glasses cannot join the room due to wrong room code
    public static ValidationResult InvalidRoomCode =>
        new ValidationResult(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.InvalidRoomCode,
                "The room code is invalid or does not exist."
            )
        );

    // VR glasses type correct room code but not in the assigned devices
    public static ValidationResult DeviceNotAssigned =>
        new ValidationResult(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.DeviceNotAuthorized,
                "The device is not assigned to this learning session."
            )
        );

    // VR glases cannot join the room at Pending, Completed, Cancelled
    public static ValidationResult SessionNotActive =>
        new ValidationResult(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.RoomNotActive,
                "The learning session is not active."
            )
        );

    // Room not found
    public static ValidationResult SessionExpiredOrNotFound =>
        new ValidationResult(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.Unspecified,
                "The learning session is not found. Please try again."
            )
        );

    public static ValidationResult Success(
      string vrLearningSessionId,
      string presetJsonContent,
      ModelVRLearningSession? modelVRLearningSession) =>
      new ValidationResult(
          IsValid: true,
          JoinRoomResponseDto: new JoinRoomResponseDto(
              JoinStatus.Success,
              "Successfully joined the learning session.",
              vrLearningSessionId: vrLearningSessionId,
              presetJsonContent: presetJsonContent
          ),
          modelVRLearningSession
      );


#pragma warning disable S125 // Sections of code should not be commented out
    //// VR glasses can join the room, but the json content is empty (later stringify)
    //public static ValidationResult Success(
    //    string vrLearningSessionId,
    //    ModelVRLearningSession modelVRLearningSession) =>
    //    new ValidationResult(
    //        IsValid: true,
    //        JoinRoomResponseDto: new JoinRoomResponseDto(
    //            JoinStatus.Success,
    //            "Successfully joined the learning session.",
    //            vrLearningSessionId,
    //            PresetJsonContent: string.Empty
    //        ),
    //        modelVRLearningSession
    //    );

#pragma warning restore S125 // Sections of code should not be commented out
}





