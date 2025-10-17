using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record CreateRoomResponseDto(
    string VRLearningSessionId)
{
    public static CreateRoomResponse MapToGrpcResponse(CreateRoomResponseDto dto)
    {
        return new CreateRoomResponse
        {
            VrLearningSessionId = dto.VRLearningSessionId
        };
    }
}
