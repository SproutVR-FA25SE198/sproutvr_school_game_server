using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Domain.Entities.Identities;

public sealed class Teacher : UserAccount
{
    // navigation properties
    public ICollection<Lesson> Lessons { get; set; } = [];
    public ICollection<VRLearningSession> VRLearningSessions { get; set; } = [];
}

