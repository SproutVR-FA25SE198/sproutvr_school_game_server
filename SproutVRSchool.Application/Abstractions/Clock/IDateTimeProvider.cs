namespace SproutVRSchool.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTimeOffset UtcDateTimeNow { get; }
    DateTimeOffset VietNamDateTimeNow { get; }

    /// <summary>
    /// Convert to Vietnam Time as DateTimeOffSet
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    DateTimeOffset ConvertToVietNamTime(DateTimeOffset dateTime);

    /// <summary>
    /// Function Overloading for converting to Vietnam Time as string
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    string ConvertToVietNamTime(DateTimeOffset? dateTime);
}
