using SproutVRSchool.Application.Abstractions.FileServices;

namespace SproutVRSchool.Infrastructure.Services.FileServices;

public class ExcelFileReader : IFileReader
{
    public Task<string> ReadAbsoluteFilePathAsync(string absoluteFilePath)
    {
        throw new NotImplementedException();
    }

    public Task<string> StringtifyAbsoluteFilePathAsync(string absoluteFilePath)
    {
        throw new NotImplementedException();
    }
}
