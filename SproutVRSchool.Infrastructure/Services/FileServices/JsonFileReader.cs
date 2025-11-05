using System.Text;
using SproutVRSchool.Application.Abstractions.FileServices;

namespace SproutVRSchool.Infrastructure.Services.FileServices;

public class JsonFileReader : IFileReader
{
    public async Task<string> ReadAbsoluteFilePathAsync(string absoluteFilePath)
    {
        try
        {
            // Check if file exists before proceeding
            bool isExist = File.Exists(absoluteFilePath);
            if (!isExist)
            {
                throw new FileNotFoundException($"File not found. {absoluteFilePath}", absoluteFilePath);
            }

            using var fileStream = new FileStream(absoluteFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var readerStream = new StreamReader(fileStream, Encoding.UTF8);

            return await readerStream.ReadToEndAsync();
        }
        catch (FileNotFoundException ex)
        {
            // Handle file not found exception specifically
            throw new Exception(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            // Handle permission-related issues
            throw new Exception($"Access to the file at {absoluteFilePath} is denied.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while reading the file.", ex);
        }
    }

    public async Task<string> StringtifyAbsoluteFilePathAsync(string absoluteFilePath)
    {
        if (!File.Exists(absoluteFilePath))
        {
            throw new FileNotFoundException($"File not found at path: {absoluteFilePath}");
        }

        // maintain all special characters in the json file
        return await File.ReadAllTextAsync(absoluteFilePath, Encoding.UTF8);
    }
}
