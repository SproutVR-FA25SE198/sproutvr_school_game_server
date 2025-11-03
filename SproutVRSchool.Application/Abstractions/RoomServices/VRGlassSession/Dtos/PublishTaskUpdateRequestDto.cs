namespace SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;

public record PublishTaskUpdateRequestDto
    (string VrLearningSessionId,
    string VrDeviceSerialNumber,
    string VrTaskId,
    bool IsCompleted,
    bool IsCorrect)
{
    public static PublishTaskUpdateRequestDto MapFromGrpcRequest(
        LearningSession.V1.TaskUpdate request,
        string vrLearningSessionId,
        string vrDeviceSerialNumber)
    {
        return new PublishTaskUpdateRequestDto(
            vrLearningSessionId,
            vrDeviceSerialNumber,
            request.VrTaskId,
            request.IsCompleted,
            request.IsCorrect);
    }
}
