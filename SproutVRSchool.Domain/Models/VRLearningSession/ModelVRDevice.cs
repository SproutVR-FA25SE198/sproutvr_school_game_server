using System.Collections.Concurrent;

namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelVRDevice
{
    public string StudentName { get; set; }
    public string VrDeviceSerialNumber { get; set; }
    public DateTimeOffset? JoinedAtUtc { get; set; }
    public ModelVRDeviceStatus Status { get; set; } = ModelVRDeviceStatus.Disconnected;
    public ConcurrentDictionary<string, ModelTaskProgress> Tasks { get; set; } = new();
}
