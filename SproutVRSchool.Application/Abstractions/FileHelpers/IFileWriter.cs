using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.FileHelpers;

public interface IFileWriter
{
    /// <summary>
    /// Return the full path of the saved file
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task<string> SaveFileUnderFolder(string folderPath, string fileName, Stream fileContent);
}
