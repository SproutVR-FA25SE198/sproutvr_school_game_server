using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Domain.Entities.Identities;

public sealed class Teacher : UserAccount
{
    // navigation properties
    public ICollection<Lesson> Lessons { get; set; } = [];
    public ICollection<VRLearningSession> VRLearningSessions { get; set; } = [];
}

