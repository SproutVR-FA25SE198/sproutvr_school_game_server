namespace SproutVRSchool.Application.Extensions;

public static class DateTimeOffsetExtensions
{
    public static string ToRoundTripUtc(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToString("o");
    }

    public static string ToRoundTripUtc(this DateTimeOffset? dateTimeOffset)
    {
        return dateTimeOffset.HasValue
            ? dateTimeOffset.Value.ToString("o") : null;
    }
}
