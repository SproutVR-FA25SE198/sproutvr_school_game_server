using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTimeOffset UtcDateTimeNow { get; }
    DateTimeOffset VietNamDateTimeNow { get; }
}
