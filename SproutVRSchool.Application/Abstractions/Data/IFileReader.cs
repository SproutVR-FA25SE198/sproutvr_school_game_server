namespace SproutVRSchool.Application.Abstractions.Data;

public interface IFileReader
{
    Task<string> ReadFileAsync(string filePath);
}
