using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed class TaskUpdatedDto
{
    public string VrDeviceSerialNumber { get; set; } = default!;
    public string VrTaskId { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public bool IsCorrect { get; set; }
    public string Status { get; set; } = ModelTaskProgressStatus.Uncompleted.ToString();
}
