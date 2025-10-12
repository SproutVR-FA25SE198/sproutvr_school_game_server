using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.Clock;

namespace SproutVRSchool.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    // ============================
    // === Zones
    // ============================

    private static readonly TimeZoneInfo VietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

    // ============================
    // === Methods
    // ============================

    public DateTimeOffset UtcDateTimeNow => DateTimeOffset.UtcNow;

    public DateTimeOffset VietNamDateTimeNow => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, VietNamTimeZone);
}
