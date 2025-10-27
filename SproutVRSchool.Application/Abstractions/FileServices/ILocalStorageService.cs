using Microsoft.AspNetCore.Http;

namespace SproutVRSchool.Application.Abstractions.FileServices;

public interface ILocalStorageService
{
    /// <summary>
    /// Save a lesson resource file (e.g. PDF, DOCX, etc.).
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    /// <returns></returns>
    Task<string> SaveLessonResourceAsync(Guid teacherId, Guid lessonId, IFormFile file);

    /// <summary>
    /// Save a VRLesson's preset file.
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="vrLessonId"></param>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    /// <returns></returns>
    Task<string> SaveVrLessonPresetAsync(Guid teacherId, Guid lessonId, Guid vrLessonId, IFormFile file);

    /// <summary>
    /// Save a VRLesson from a stream
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="vrLessonId"></param>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    /// <returns></returns>
    Task<string> SaveVrLessonPresetAsync(Guid teacherId, Guid lessonId, Guid vrLessonId, string fileName, Stream fileContent);

    /// <summary>
    /// Save a VRLesson's image file.
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="vrLessonId"></param>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    /// <returns></returns>
    Task<string> SaveVrLessonImageAsync(Guid teacherId, Guid lessonId, Guid vrLessonId, IFormFile file);

    /// <summary>
    /// Delete a file in the local storage, by giving the public url file path
    /// </summary>
    /// <param name="publicRelativeFilePath"></param>
    Task DeleteFileInLocalStorageAsync(string publicRelativeFilePath);
}
