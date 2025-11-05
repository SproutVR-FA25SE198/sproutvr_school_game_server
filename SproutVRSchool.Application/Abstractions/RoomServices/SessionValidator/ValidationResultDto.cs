using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain.Models.VRLearningSession;
using static LearningSession.V1.JoinRoomResponse.Types;

namespace SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;

/// <summary>
/// This interface will validate the learning session state
/// </summary>
public record ValidationResultDto(
    bool IsValid,
    JoinRoomResponseDto? JoinRoomResponseDto = null,
    ModelVRLearningSession? ModelVRLearningSession = null  // after validation, return the learning session info to avoid fetching twice
)
{
    // VR glasses cannot join the room due to wrong room code
    public static ValidationResultDto InvalidRoomCode =>
        new ValidationResultDto(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.InvalidRoomCode,
                "The room code is invalid or does not exist."
            )
        );

    // VR glasses type correct room code but not in the assigned devices
    public static ValidationResultDto DeviceNotAssigned =>
        new ValidationResultDto(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.DeviceNotAuthorized,
                "The device is not assigned to this learning session."
            )
        );

    // VR glases cannot join the room at Pending, Completed, Cancelled
    public static ValidationResultDto SessionNotActive =>
        new ValidationResultDto(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.RoomNotActive,
                "The learning session is not active."
            )
        );

    // Room not found
    public static ValidationResultDto SessionExpiredOrNotFound =>
        new ValidationResultDto(
            IsValid: false,
            JoinRoomResponseDto: new JoinRoomResponseDto(
                JoinStatus.Unspecified,
                "The learning session is not found. Please try again."
            )
        );

    // VR glasses cannot rejoin the room
    public static ValidationResultDto AlreayJoined =>
       new ValidationResultDto(
           IsValid: false,
           JoinRoomResponseDto: new JoinRoomResponseDto(
               JoinStatus.AlreadyJoined,
               "The device cannot rejoin the learning session."
           )
       );

    public static ValidationResultDto Success(
      string vrLearningSessionId,
      string presetJsonContent,
      ModelVRLearningSession? modelVRLearningSession) =>
      new ValidationResultDto(
          IsValid: true,
          JoinRoomResponseDto: new JoinRoomResponseDto(
              JoinStatus.Success,
              "Successfully joined the learning session.",
              vrLearningSessionId: vrLearningSessionId,
              presetJsonContent: presetJsonContent
          ),
          modelVRLearningSession
      );
}





