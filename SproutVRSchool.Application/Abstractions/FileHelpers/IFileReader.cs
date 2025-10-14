namespace SproutVRSchool.Application.Abstractions.FileHelpers;

public interface IFileReader
{
    Task<string> ReadFileAsync(string filePath);
}
