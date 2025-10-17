using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record CancelRoomResponseDto
    (string Message)
{
    public static CancelRoomResponse MapToGrpcResponse(CancelRoomResponseDto dto)
    {
        return new CancelRoomResponse
        {
            Message = dto.Message
        };
    }
}
