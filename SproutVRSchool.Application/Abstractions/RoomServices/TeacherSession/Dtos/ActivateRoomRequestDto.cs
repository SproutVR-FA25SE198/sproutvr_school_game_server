using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record ActivateRoomRequestDto(
    string LearningSessionId,
    string VrLessonId,
    DateTimeOffset StartTimeUtc,
    IEnumerable<AssignedDeviceRequestDto> AssignedDeviceSerials)
{
    public static ActivateRoomRequestDto MapFromGrpcRequest(ActivateRoomRequest activateRoomRequest)
    {
        return new ActivateRoomRequestDto(
            activateRoomRequest.VrLearningSessionId,
            activateRoomRequest.VrLessonId,
            activateRoomRequest.StartTimeAtUtc.ToDateTimeOffset(),
            activateRoomRequest.AssignedDeviceSerials.Select(e => AssignedDeviceRequestDto.MapFromGrpcRequest(e))
            );
    }
}
