using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.SearchVRLessons;

public record AuthorizedSearchVRLessonsQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedVRLessonLessonQueryResponseDto Lesson { get; init; }
    public AuthorizedVRLessonMapQueryResponseDto Map { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public TimeSpan MaxDuration { get; init; }
    public string? PresetJsonRelativeFilePath { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}

public record AuthorizedVRLessonLessonQueryResponseDto(
    Guid Id,
    string Name,
    string Description
);

public record AuthorizedVRLessonMapQueryResponseDto(
    Guid Id,
    string Name,
    string MapCode,
    string ImageUrl,
    string PreviewUrl
);
