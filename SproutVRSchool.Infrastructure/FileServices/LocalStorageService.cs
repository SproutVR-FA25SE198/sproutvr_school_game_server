using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Infrastructure.FileServices;

public sealed class LocalStorageService : ILocalStorageService, IPathService
{
    // =============================
    // === Get Files & Paths
    // =============================

    public string GetTeacherAbsoluteFolderPath(Guid teacherId)
    {
        return Path.Combine(AppCts.FilePaths.StorageRootPath, teacherId.ToString());
    }

    public string GetLessonAbsoluteFolderPath(Guid teacherId, Guid lessonId)
    {
        return Path.Combine(GetTeacherAbsoluteFolderPath(teacherId), lessonId.ToString());
    }

    public string GetLessonResourcesAbsoluteFolderPath(Guid teacherId, Guid lessonId)
    {
        return Path.Combine(GetLessonAbsoluteFolderPath(teacherId, lessonId), AppCts.FilePaths.FOLDER_NAME_RESOURCES);
    }

    public string GetVRLessonAbsoluteFolderPath(Guid teacherId, Guid lessonId, Guid vrLessonId)
    {
        return Path.Combine(GetLessonAbsoluteFolderPath(teacherId, lessonId), vrLessonId.ToString());
    }

    public string GetVRLessonImagesAbsoluteFolderPath(Guid teacherId, Guid lessonId, Guid vrLessonId)
    {
        return Path.Combine(GetVRLessonAbsoluteFolderPath(teacherId, lessonId, vrLessonId), AppCts.FilePaths.FOLDER_NAME_IMAGES);
    }

    public string GetVRLessonPresetsAbsoluteFolderPath(Guid teacherId, Guid lessonId, Guid vrLessonId)
    {
        return Path.Combine(GetVRLessonAbsoluteFolderPath(teacherId, lessonId, vrLessonId), AppCts.FilePaths.FOLDER_NAME_PRESETS);
    }

    // =============================
    // === Save Files & Resources
    // =============================

    public async Task<string> SaveLessonResourceAsync(Guid teacherId, Guid lessonId, IFormFile file)
    {
        string lessonResourcesPath = GetLessonResourcesAbsoluteFolderPath(teacherId, lessonId);

        string fileName = file.FileName;
        using Stream fileContent = file.OpenReadStream();

        // write content into the  
        await WriteFileAsync(lessonResourcesPath, fileName, fileContent);

        // return the relative path
        string relativeUrlPath = Path.Combine(
            teacherId.ToString(),
            lessonId.ToString(),
            AppCts.FilePaths.FOLDER_NAME_RESOURCES,
            fileName);

        return ConvertToPublicPath(relativeUrlPath);
    }

    public async Task<string> SaveVrLessonImageAsync(Guid teacherId, Guid lessonId, Guid vrLessonId, IFormFile file)
    {
        string vrLessonImagesPath = GetVRLessonImagesAbsoluteFolderPath(teacherId, lessonId, vrLessonId);

        string fileName = file.FileName;
        using Stream fileContent = file.OpenReadStream();

        // write content into the  
        await WriteFileAsync(vrLessonImagesPath, fileName, fileContent);

        // return the relative path
        string relativeUrlPath = Path.Combine(
            teacherId.ToString(),
            lessonId.ToString(),
            vrLessonId.ToString(),
            AppCts.FilePaths.FOLDER_NAME_IMAGES,
            fileName);

        return ConvertToPublicPath(relativeUrlPath);
    }

    public async Task<string> SaveVrLessonPresetAsync(Guid teacherId, Guid lessonId, Guid vrLessonId, IFormFile file)
    {
        string vrLessonPresentPath = GetVRLessonPresetsAbsoluteFolderPath(teacherId, lessonId, vrLessonId);

        string fileName = file.FileName;
        using Stream fileContent = file.OpenReadStream();

        // write content into the  
        await WriteFileAsync(vrLessonPresentPath, fileName, fileContent);

        // return the relative path
        string relativeUrlPath = Path.Combine(
            teacherId.ToString(),
            lessonId.ToString(),
            vrLessonId.ToString(),
            AppCts.FilePaths.FOLDER_NAME_PRESETS,
            fileName);

        return ConvertToPublicPath(relativeUrlPath);
    }

    // =============================
    // === Helpers
    // =============================

    /// <summary>
    /// This is a function that create a file at the given folder path
    /// </summary>
    /// <param name="absoluteFolderPath"></param>
    /// <param name="fileName"></param>
    /// <param name="fileContent"></param>
    /// <returns></returns>
    private static async Task WriteFileAsync(string absoluteFolderPath, string fileName, Stream fileContent)
    {
        Directory.CreateDirectory(absoluteFolderPath);
        string absoluteFilePath = Path.Combine(absoluteFolderPath, fileName);

        using var fileStream = new FileStream(absoluteFilePath, FileMode.Create, FileAccess.Write);
        await fileStream.CopyToAsync(fileContent);
    }

    /// <summary>
    /// Convert a Windows-style path to a public URL path
    /// e.g. C:\ProgramData\SproutVRSchool\Content\{teacher_id}\{lesson_id}\Resources\file.pdf
    /// e.g. Into: /content/{teacher_id}/{lesson_id}/Resources/file.pdf
    /// </summary>
    /// <param name="windowPath"></param>
    /// <returns></returns>
    private static string ConvertToPublicPath(string windowPath)
    {
        return $"{AppCts.FilePaths.PREFIX_PUBLIC_CONTENT_PATH}/{windowPath.Replace('\\', '/')}";
    }
}
