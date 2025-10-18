using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
