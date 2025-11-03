using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Queries.GetLessonById;

public record TeacherGetLessonByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public GetLessonByIdSubjectResponseDto Subject { get; init; }
    public GetLessonByIdTeacherResponseDto Teacher { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ResourceRelativeFilePath { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
}

public record GetLessonByIdSubjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
}

public record GetLessonByIdTeacherResponseDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
}
