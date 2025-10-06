using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Domain.Entities.MasterSubjects;

public sealed class MasterSubject : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public MasterSubjectStatus Status { get; set; }

    // navigation property
    public ICollection<Subject> Subjects { get; set; } = [];
}
