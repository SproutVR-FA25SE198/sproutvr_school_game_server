using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;

namespace SproutVRSchool.Application.Exceptions;

public sealed class SvrFileNotSupportedException : Exception
{
    public string FileName { get; }
    public SvrFileNotSupportedException(string? message) : base(message)
    {
    }

    public SvrFileNotSupportedException() :
        base("The specified file type is not supported")
    {

    }
}
