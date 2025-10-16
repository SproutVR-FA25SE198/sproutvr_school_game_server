using SproutVRSchool.Application.Abstractions.FileHelpers;

namespace SproutVRSchool.Infrastructure.FileHelpers;

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
