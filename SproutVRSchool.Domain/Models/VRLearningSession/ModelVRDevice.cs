using System.Collections.Concurrent;

namespace SproutVRSchool.Domain.Models.VRLearningSession;

public class ModelVRDevice
{
    public string DeviceName { get; set; }
    public string SerialNumber { get; set; }
    public ModelVRDeviceStatus Status { get; set; } = ModelVRDeviceStatus.Disconnected;
    public DateTimeOffset JoinedAtUtc { get; set; }
    public ConcurrentDictionary<string, ModelTaskProgress> Tasks { get; set; } = new();
}
