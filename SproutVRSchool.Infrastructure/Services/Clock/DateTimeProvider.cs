using System.Globalization;
using SproutVRSchool.Application.Abstractions.Clock;

namespace SproutVRSchool.Infrastructure.Services.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    // ============================
    // === Zones
    // ============================

    private readonly TimeZoneInfo VietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");



    // ============================
    // === Methods
    // ============================

    public DateTimeOffset ConvertToVietNamTime(DateTimeOffset dateTime) => TimeZoneInfo.ConvertTime(dateTime, VietNamTimeZone);

    public string ConvertToVietNamTime(DateTimeOffset? dateTime)
    {
        // 1. Check if the value is null
        if (dateTime is null)
        {
            return string.Empty;
        }

        // 2. If it's not null, convert it
        DateTimeOffset vietNamTime = TimeZoneInfo.ConvertTime(dateTime.Value, VietNamTimeZone);

        // 3. Return it as a standard ISO 8601 string
        return vietNamTime.ToString("o", CultureInfo.InvariantCulture);
    }

    public DateTimeOffset UtcDateTimeNow => DateTimeOffset.UtcNow;

    public DateTimeOffset VietNamDateTimeNow => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, VietNamTimeZone);

}
