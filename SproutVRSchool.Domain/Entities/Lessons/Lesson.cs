using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Domain.Entities.Lessons;

public sealed class Lesson : BaseEntity
{
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ResourceUrl { get; set; }
    public LessonStatus Status { get; set; }

    // navigation property
    public Subject Subject { get; set; }
    public Teacher Teacher { get; set; }
    public ICollection<VRLesson> VRLessons { get; set; } = [];
}
