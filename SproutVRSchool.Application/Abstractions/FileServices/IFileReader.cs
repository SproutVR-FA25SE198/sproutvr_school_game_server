namespace SproutVRSchool.Application.Abstractions.FileServices;

public interface IFileReader
{
    Task<string> ReadFileAsync(string filePath);
}
