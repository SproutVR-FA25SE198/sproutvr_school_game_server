using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.Services.TeacherSession.Dtos;

public record ActivateRoomRequestDto(
    string LearningSessionId,
    DateTimeOffset StartTimeUtc,
    int DurationInMinutes,
    IEnumerable<string> AssignedDeviceSerials)
{
    public static ActivateRoomRequestDto MapFromGrpcRequest(ActivateRoomRequest activateRoomRequest)
    {
        return new ActivateRoomRequestDto(
            activateRoomRequest.VrLearningSessionId,
            activateRoomRequest.StartTimeAtUtc.ToDateTimeOffset(),
            (int)activateRoomRequest.DurationInMinutes.ToTimeSpan().TotalMinutes,
            activateRoomRequest.AssignedDeviceSerials);
    }
}
