using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
