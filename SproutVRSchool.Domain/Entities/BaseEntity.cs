using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreateAtUtc { get; set; }
    public DateTimeOffset UpdateAtUtc { get; set; }
}
