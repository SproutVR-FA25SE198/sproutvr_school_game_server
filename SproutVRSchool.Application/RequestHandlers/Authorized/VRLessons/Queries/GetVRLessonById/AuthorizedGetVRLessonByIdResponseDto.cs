using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;

public record AuthorizedGetVRLessonByIdResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetVRLessonByIdLessonResponseDto Lesson { get; init; }
    public AuthorizedGetVRLessonByIdMapResponseDto Map { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public TimeSpan Duration { get; init; }
    public string? PresetJsonRelativeFilePath { get; init; }
    public StatusDto Status { get; init; }

    public IReadOnlyList<AuthorizedGetVRLessonByIdTaskResponseDto> Tasks { get; init; } = [];
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}

public class AuthorizedGetVRLessonByIdLessonResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}

public record AuthorizedGetVRLessonByIdTaskResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetVRLessonByIdTaskLocationResponseDto TaskLocation { get; init; }
    public AuthorizedGetVRLessonByIdMapObjectResponseDto MapObject { get; init; }
    public AuthorizedGetVRLessonByIdActivityTypeResponseDto ActivityType { get; init; }
    public int TaskNumber { get; init; }
    public string Description { get; init; }
}

public class AuthorizedGetVRLessonByIdActivityTypeResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ActivityCode { get; init; }
}

public record AuthorizedGetVRLessonByIdMapObjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ObjectCode { get; init; }
    public string ImageUrl { get; init; }
}

public record AuthorizedGetVRLessonByIdTaskLocationResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string LocationCode { get; init; }
    public string ImageUrl { get; init; }
}

public record AuthorizedGetVRLessonByIdMapResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string MapCode { get; init; }
    public string ImageUrl { get; init; }
}
