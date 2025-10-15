using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.Services.TeacherSession.Dtos;

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
