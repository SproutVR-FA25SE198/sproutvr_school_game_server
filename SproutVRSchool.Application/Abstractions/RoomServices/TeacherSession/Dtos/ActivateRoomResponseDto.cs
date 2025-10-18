using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record ActivateRoomResponseDto(
    string RoomCode)
{
    public static ActivateRoomResponse MapToGrpcResponse(ActivateRoomResponseDto resultDto)
    {
        return new ActivateRoomResponse
        {
            RoomCode = resultDto.RoomCode
        };
    }
}
