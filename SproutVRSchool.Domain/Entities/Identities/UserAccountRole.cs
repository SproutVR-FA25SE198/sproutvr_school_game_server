using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SproutVRSchool.Domain.Entities.Identities;

public sealed class UserAccountRole : IdentityRole<Guid>
{
}
