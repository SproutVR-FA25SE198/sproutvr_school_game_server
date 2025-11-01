using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Exceptions;

public sealed class SvrAlreadyInstalledException : Exception
{
    public SvrAlreadyInstalledException(string message) : base(message)
    {
    }
}
