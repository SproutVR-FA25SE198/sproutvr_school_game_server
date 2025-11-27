using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.Abstractions.FileServices;

public interface IFileValidationService
{
    /// <summary>
    /// Check to see if the file is valid
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool IsFileValid(IFormFile file);

    /// <summary>
    /// Check if it's an excel file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool IsExcelFile(IFormFile file);

    /// <summary>
    /// Check if it's a PDF file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool IsPdfFile(IFormFile file);
}
