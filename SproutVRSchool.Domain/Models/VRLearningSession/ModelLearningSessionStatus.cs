using System.Text.Json.Serialization;

namespace SproutVRSchool.Domain.Models.VRLearningSession;

public enum ModelLearningSessionStatus
{
    Pending = 1,
    Active = 2,
    Completed = 3,
    Cancelled = 4
}
