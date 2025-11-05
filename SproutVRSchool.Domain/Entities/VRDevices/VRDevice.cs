using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;

namespace SproutVRSchool.Domain.Entities.VRDevices;

public sealed class VRDevice : BaseEntity
{
    // ==============================
    // === Fields
    // ==============================

    public string Name { get; set; }
    public VRDeviceStatus Status { get; set; }
    public string SerialNumber { get; set; }

    // Navigation properties
    public ICollection<VRDeviceSessionSummary> VRDeviceSessionSummaries { get; set; }
    public ICollection<VRDeviceTaskProgress> VRDeviceTaskProgresses { get; set; }

    // ==============================
    // === Methods
    // ==============================

    public static VRDevice Create(string Name, string SerialNumber)
    {
        return new VRDevice
        {
            Id = Guid.NewGuid(),
            Name = Name,
            SerialNumber = SerialNumber,
            Status = VRDeviceStatus.Available,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };
    }
}
