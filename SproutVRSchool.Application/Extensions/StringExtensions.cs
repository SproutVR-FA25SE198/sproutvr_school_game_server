namespace SproutVRSchool.Application.Extensions;

public static class StringExtensions
{
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
}
