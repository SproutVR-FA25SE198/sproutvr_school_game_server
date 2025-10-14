using SproutVRSchool.Application.Abstractions.Clock;

namespace SproutVRSchool.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    // ============================
    // === Zones
    // ============================

    private readonly TimeZoneInfo VietNamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

    public DateTimeOffset ConvertToVietNamTime(DateTimeOffset dateTime) =>
    TimeZoneInfo.ConvertTime(dateTime, VietNamTimeZone);

    // ============================
    // === Methods
    // ============================

    public DateTimeOffset UtcDateTimeNow => DateTimeOffset.UtcNow;

    public DateTimeOffset VietNamDateTimeNow => TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, VietNamTimeZone);

}
