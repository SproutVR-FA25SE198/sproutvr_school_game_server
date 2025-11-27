using SproutVRSchool.Domain.Models.VRLearningSession;

namespace SproutVRSchool.Application.Abstractions.RoomServices.TeacherSessionState.Dtos.StreamRoomState;

// UNDONE: appcts for status
public sealed class DeviceDisconnectedDto
{
    public string VrDeviceSerialNumber { get; set; } = default!;
    public string ConnectionStatus { get; set; } = ModelVRDeviceStatus.Disconnected.ToString();
}
