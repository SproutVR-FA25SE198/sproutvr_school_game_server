using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.Data;

namespace SproutVRSchool.Infrastructure.FileServices;

public class ExcelFileReader : IFileReader
{
    public Task<string> ReadFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }
}
