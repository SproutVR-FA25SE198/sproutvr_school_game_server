using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed class RoomEndedDto
{
    public string Reason { get; set; } = "Time's up";
    public string RoomStatus { get; set; } = ModelVRLearningSessionStatus.Cancelled.ToString();
}
