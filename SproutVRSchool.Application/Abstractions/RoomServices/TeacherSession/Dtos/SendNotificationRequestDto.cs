using LearningSession.V1;
using static LearningSession.V1.NotificationSignal.Types;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession.Dtos;

public record SendNotificationRequestDto(
    string VRLearningSessionId,
    string Text,
    Severity Severity)
{
    public static SendNotificationRequestDto MapFromGrpcRequest(SendNotificationRequest request)
    {
        return new SendNotificationRequestDto(
            request.VrLearningSessionId,
            request.Notification.Text,
            request.Notification.Severity);
    }
}
