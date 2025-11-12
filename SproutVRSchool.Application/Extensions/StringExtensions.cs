namespace SproutVRSchool.Application.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Convert string value to boolean
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ToBoolean(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
               value == "1" ||
               value.Equals("yes", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Get the eventType and message
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static (string? identifier, string? eventType, string? message, bool isEventType) DeparseRedisEventMessage(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return (null, null, null, false);
        }

        string[] parts = value.Trim().Split(":", 3);
        if (parts.Length < 2)
        {
            return (null, null, null, false);
        }

        string identifier = parts[0];
        string eventType = parts[1]!.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        string message = parts[2];

        return (identifier, eventType, message, true);
    }

    /// <summary>
    /// Parse to Redis Event Type Message
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="eventType"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string ToRedisEventTypeMessage(this string identifier, string eventType, string message)
    {
        return $"{identifier}:{eventType}:{message}";
    }
}
