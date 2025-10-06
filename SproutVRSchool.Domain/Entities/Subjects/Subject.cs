using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Domain.Entities.Subjects;

public sealed class Subject : BaseEntity
{
    public Guid MasterSubjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public SubjectStatus Status { get; set; }

    // navigation property
    public MasterSubject MasterSubject { get; set; }
    public ICollection<Map> Maps { get; set; } = [];
}
