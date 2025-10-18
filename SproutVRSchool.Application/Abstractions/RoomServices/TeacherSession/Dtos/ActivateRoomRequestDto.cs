using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

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
