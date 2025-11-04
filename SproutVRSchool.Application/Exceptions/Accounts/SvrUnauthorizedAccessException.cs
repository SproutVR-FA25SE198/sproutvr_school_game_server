using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Exceptions.Accounts;

public sealed class SvrUnauthorizedAccessException : Exception
{
    public SvrUnauthorizedAccessException(string? message) : base(message)
    {
    }
}
