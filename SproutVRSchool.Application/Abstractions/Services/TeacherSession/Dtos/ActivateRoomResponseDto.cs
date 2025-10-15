using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.Services.TeacherSession.Dtos;

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
