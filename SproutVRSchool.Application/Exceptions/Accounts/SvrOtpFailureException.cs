using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrOtpFailureException : Exception
{
    public SvrOtpFailureException(string? message) : base(message)
    {
    }
}
