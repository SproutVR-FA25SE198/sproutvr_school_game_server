namespace SproutVRSchool.Application.Abstractions.FileHelpers;

public interface IFileReader
{
    /// <summary>
    /// Read and maintain the content of the file (including
    /// </summary>
    /// <param name="absoluteFilePath"></param>
    /// <returns></returns>
    Task<string> ReadAbsoluteFilePathAsync(string absoluteFilePath);

    /// <summary>
    /// Stringtify the JSON file content by removing all unnecessary spaces, new lines, tabs, etc.
    /// </summary>
    /// <param name="absoluteFilePath"></param>
    /// <returns></returns>
    Task<string> StringtifyAbsoluteFilePathAsync(string absoluteFilePath);
}
