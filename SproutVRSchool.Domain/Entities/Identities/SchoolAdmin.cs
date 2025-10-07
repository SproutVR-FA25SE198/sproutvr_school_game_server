using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Domain.Entities.Identities;

public sealed class SchoolAdmin : UserAccount
{
    public Guid OrganizationId { get; set; }
}
