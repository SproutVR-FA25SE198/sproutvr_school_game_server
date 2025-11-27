using LearningSession.V1;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record ActivateRoomRequestDto(
    string LearningSessionId,
    string VrLessonId,
    int RoomDurationInMinutes,
    IEnumerable<AssignedDeviceRequestDto> AssignedDeviceSerials)
{
    public static ActivateRoomRequestDto MapFromGrpcRequest(ActivateRoomRequest activateRoomRequest)
    {
        return new ActivateRoomRequestDto(
            activateRoomRequest.VrLearningSessionId,
            activateRoomRequest.VrLessonId,
            activateRoomRequest.RoomDurationInMinutes,
            activateRoomRequest.AssignedDeviceSerials.Select(e => AssignedDeviceRequestDto.MapFromGrpcRequest(e))
        );
    }
}
