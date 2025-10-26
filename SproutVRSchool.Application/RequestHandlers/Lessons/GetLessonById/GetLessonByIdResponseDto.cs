using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.GetLessonById;

public record GetLessonByIdResponseDto
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
