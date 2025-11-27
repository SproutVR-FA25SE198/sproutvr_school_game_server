namespace SproutVRSchool.Application.Abstractions.FileServices;


/*
 
Local storage path's structure

Content/
    {teacher_id}

    /{lesson_id}

    /Resources/bai1.pdf

       /{vrLesson_id}

              /Images/image1.jpg

              /Presets/preset.jpb
 
 */
public interface IPathService
{
    /// <summary>
    /// e.g., C:\ProgramData\SproutVRSchool\Content\{teacher_id}\
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    public string GetTeacherAbsoluteFolderPath(Guid teacherId);

    /// <summary>
    /// e.g., C:\ProgramData\SproutVRSchool\Content\{teacher_id}\{lesson_id}\
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <returns></returns>
    public string GetLessonAbsoluteFolderPath(Guid teacherId, Guid lessonId);

    /// <summary>
    /// e.g., C:\ProgramData\SproutVRSchool\Content\{teacher_id}\{lesson_id}\{vrLesson_id}
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="vrLessonId"></param>
    /// <returns></returns>
    public string GetVRLessonAbsoluteFolderPath(Guid teacherId, Guid lessonId, Guid vrLessonId);

    /// <summary>
    /// e.g., C:\ProgramData\SproutVRSchool\Content\{teacher_id}\{lesson_id}\Resources
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <returns></returns>
    public string GetLessonResourcesAbsoluteFolderPath(Guid teacherId, Guid lessonId);

    /// <summary>
    /// e.g., C:\ProgramData\SproutVRSchool\Content\{teacher_id}\{lesson_id}\Images
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="vrLessonId"></param>
    /// <returns></returns>
    public string GetVRLessonImagesAbsoluteFolderPath(Guid teacherId, Guid lessonId, Guid vrLessonId);

    /// <summary>
    /// e.g., C:\ProgramData\SproutVRSchool\Content\{teacher_id}\{lesson_id}\Presets
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="lessonId"></param>
    /// <param name="vrLessonId"></param>
    /// <returns></returns>
    public string GetVRLessonPresetsAbsoluteFolderPath(Guid teacherId, Guid lessonId, Guid vrLessonId);
}
