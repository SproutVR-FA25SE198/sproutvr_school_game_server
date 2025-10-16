using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.FileHelpers;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Infrastructure.FileHelpers;

public sealed class PresetFileWriter : IFileWriter
{
    public async Task<string> SaveFileUnderFolder(string folderPath, string fileName, Stream fileContent)
    {
        Directory.CreateDirectory(folderPath);

        string fullFilePath = Path.Combine(folderPath, fileName);

        // open 1 file stream to copy the content of the file to the destionation file
        using var fileStream = new FileStream(fullFilePath, FileMode.Create);
        await fileContent.CopyToAsync(fileStream);

        return fullFilePath;
    }

    public static string GetFullTeacherPresetFilePath(Guid teacherId, string fileName)
    {
        string teacherFolder = Path.Combine(
            AppContext.BaseDirectory,
            AppCts.PresetFilePaths.PresetFolderPath,
            teacherId.ToString());

        return Path.Combine(teacherFolder, fileName);
    }
}
