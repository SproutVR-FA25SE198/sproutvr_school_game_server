namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.GetVRTaskById;

public record AuthorizedGetVRTaskByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetVRTaskByIdQueryTaskLocationResponseDto TaskLocation { get; init; }
    public AuthorizedGetVRTaskByIdQueryMapObjectResponseDto MapObject { get; init; }
    public AuthorizedGetVRTaskByIdQueryActivityTypeResponseDto ActivityType { get; init; }
    public AuthorizedGetVRTaskByIdQueryVRLessonResponseDto VRLesson { get; init; }
    public int TaskNumber { get; init; }
    public string? Question { get; init; }
    public string Description { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; } // Included as requested
}

public record AuthorizedGetVRTaskByIdQueryTaskLocationResponseDto(
    Guid Id,
    string Name,
    string LocationCode
);

public record AuthorizedGetVRTaskByIdQueryMapObjectResponseDto(
    Guid Id,
    string Name,
    string ObjectCode
);

public record AuthorizedGetVRTaskByIdQueryActivityTypeResponseDto(
    Guid Id,
    string Name,
    string ActivityCode
);

public record AuthorizedGetVRTaskByIdQueryVRLessonResponseDto(
    Guid Id,
    string Name
);
