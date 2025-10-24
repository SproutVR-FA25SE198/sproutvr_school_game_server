using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;

namespace SproutVRSchool.Application.Exceptions;

public sealed class FileNotSupportedException : Exception
{
    public string FileName { get; }
    public FileNotSupportedException(string? message) : base(message)
    {
    }

    public FileNotSupportedException() :
        base("The specified file type is not supported")
    {

    }
}
