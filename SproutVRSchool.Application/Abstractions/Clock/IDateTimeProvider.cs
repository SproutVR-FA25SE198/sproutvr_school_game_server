namespace SproutVRSchool.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTimeOffset UtcDateTimeNow { get; }
    DateTimeOffset VietNamDateTimeNow { get; }
    DateTimeOffset ConvertToVietNamTime(DateTimeOffset dateTime);
}
