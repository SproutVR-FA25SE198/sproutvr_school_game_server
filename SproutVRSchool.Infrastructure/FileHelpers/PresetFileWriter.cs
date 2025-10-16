using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Application.Abstractions.FileHelpers;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.FileHelpers;

public sealed class PresetFileWriter : IFileWriter
{
    private readonly string _basePath = AppContext.BaseDirectory;

    public async Task<string> SaveFileUnderFolder(Guid teacherId, string fileName, Stream fileContent)
    {
        string absoluteFolderPath = Path.Combine(
            _basePath,
            AppCts.PresetFilePaths.PresetFolderPath,
            teacherId.ToString());

        string absoluteFilePath = Path.Combine(absoluteFolderPath, fileName);

        // open 1 file stream to copy the content of the file to the destionation file
        Directory.CreateDirectory(absoluteFolderPath);
        using var fileStream = new FileStream(absoluteFilePath, FileMode.Create);
        await fileContent.CopyToAsync(fileStream);

        return Path.Combine(AppCts.PresetFilePaths.PresetFolderPath, teacherId.ToString(), fileName);
    }
}
