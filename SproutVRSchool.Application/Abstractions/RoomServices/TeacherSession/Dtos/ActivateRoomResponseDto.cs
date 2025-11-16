using LearningSession.V1;
using SproutVRSchool.Application.Extensions;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record ActivateRoomResponseDto(
    DateTimeOffset StartTimeAtUtc,
    DateTimeOffset EndTimeAtUtc,
    string RoomCode)
{
    public static ActivateRoomResponse MapToGrpcResponse(ActivateRoomResponseDto resultDto)
    {
        return new ActivateRoomResponse
        {
            RoomCode = resultDto.RoomCode,
            EndTimeAtUtc = resultDto.EndTimeAtUtc.ToRoundTripUtc(),
            StartTimeAtUtc = resultDto.StartTimeAtUtc.ToRoundTripUtc()
        };
    }
}
