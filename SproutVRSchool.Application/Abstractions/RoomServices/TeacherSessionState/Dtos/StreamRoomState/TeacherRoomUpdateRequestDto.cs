using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed record TeacherRoomUpdateRequestDto(string VrLearningSessionId)
{
    public static TeacherRoomUpdateRequestDto MapFromGrpcRequest(TeacherRoomStreamRequest request)
    {
        return new TeacherRoomUpdateRequestDto(
            VrLearningSessionId: request.VrLearningSessionId
        );
    }
}
