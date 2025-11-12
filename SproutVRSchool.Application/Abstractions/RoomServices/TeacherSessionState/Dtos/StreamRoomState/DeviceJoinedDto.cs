using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

public sealed class DeviceJoinedDto
{
    public string VrDeviceSerialNumber { get; set; } = default!;
    public string ConnectionStatus { get; set; } = ModelVRDeviceStatus.Connected.ToString();
}
