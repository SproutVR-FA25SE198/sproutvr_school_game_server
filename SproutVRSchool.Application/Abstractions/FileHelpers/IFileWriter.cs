using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SproutVRSchool.Application.Abstractions.FileHelpers;

public interface IFileWriter
{
    /// <summary>
    /// Save file under the absolute path and return the relative path
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task<string> SaveFileUnderFolder(Guid teacherId, string fileName, Stream fileContent);
}
