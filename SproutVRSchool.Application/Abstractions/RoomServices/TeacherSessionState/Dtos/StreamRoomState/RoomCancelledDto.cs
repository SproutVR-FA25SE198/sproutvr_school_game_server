using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed class RoomCancelledDto
{
    public string Reason { get; set; } = "Room has been cancelled";
    public string RoomStatus { get; set; } = ModelVRLearningSessionStatus.Cancelled.ToString();
}

