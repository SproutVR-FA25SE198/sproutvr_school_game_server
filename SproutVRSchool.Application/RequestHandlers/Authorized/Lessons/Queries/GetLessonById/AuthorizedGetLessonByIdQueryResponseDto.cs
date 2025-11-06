using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.GetLessonById;

public record AuthorizedGetLessonByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetLessonByIdSubjectResponseDto Subject { get; init; }
    public AuthorizedGetLessonByIdTeacherResponseDto Teacher { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ResourceRelativeFilePath { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; set; }
}

public record AuthorizedGetLessonByIdSubjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
}

public record AuthorizedGetLessonByIdTeacherResponseDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
}
