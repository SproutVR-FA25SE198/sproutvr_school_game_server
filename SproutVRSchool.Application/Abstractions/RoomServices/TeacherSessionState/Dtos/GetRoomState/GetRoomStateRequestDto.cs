using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.GetRoomState;

public sealed class GetRoomStateRequestDto
{
    public string VRLearningSessionId { get; init; }

    public static GetRoomStateRequestDto MapFromGrpcRequest(GetRoomStateRequest request) =>
        new GetRoomStateRequestDto
        {
            VRLearningSessionId = request.VrLearningSessionId
        };
}


