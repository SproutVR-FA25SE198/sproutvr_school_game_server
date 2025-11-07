using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;

public record AuthorizedGetVRLessonByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetVRLessonByIdQueryLessonResponseDto Lesson { get; init; }
    public AuthorizedGetVRLessonByIdQueryMapResponseDto Map { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public TimeSpan Duration { get; init; }
    public string? PresetJsonRelativeFilePath { get; init; }
    public StatusDto Status { get; init; }

    public IReadOnlyList<AuthorizedGetVRLessonByIdQueryTaskResponseDto> Tasks { get; init; } = [];
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}

public class AuthorizedGetVRLessonByIdQueryLessonResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}

public record AuthorizedGetVRLessonByIdQueryTaskResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetVRLessonByIdQueryTaskLocationResponseDto TaskLocation { get; init; }
    public AuthorizedGetVRLessonByIdQueryMapObjectResponseDto MapObject { get; init; }
    public AuthorizedGetVRLessonByIdQueryActivityTypeResponseDto ActivityType { get; init; }
    public int TaskNumber { get; init; }
    public string Description { get; init; }
}

public class AuthorizedGetVRLessonByIdQueryActivityTypeResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ActivityCode { get; init; }
}

public record AuthorizedGetVRLessonByIdQueryMapObjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ObjectCode { get; init; }
    public string ImageUrl { get; init; }
}

public record AuthorizedGetVRLessonByIdQueryTaskLocationResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string LocationCode { get; init; }
    public string ImageUrl { get; init; }
}

public record AuthorizedGetVRLessonByIdQueryMapResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string MapCode { get; init; }
    public string ImageUrl { get; init; }
}
