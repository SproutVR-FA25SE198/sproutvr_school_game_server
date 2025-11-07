using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.SearchVRTasks;

public record AuthorizedSearchVRTasksQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedSearchVRTasksQueryTaskLocationResponseDto TaskLocation { get; init; }
    public AuthorizedSearchVRTasksQueryMapObjectResponseDto MapObject { get; init; }
    public AuthorizedSearchVRTasksQueryActivityTypeResponseDto ActivityType { get; init; }
    public AuthorizedSearchVRTasksQueryVRLessonResponseDto VRLesson { get; init; }
    public int TaskNumber { get; init; }
    public string Description { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}

public record AuthorizedSearchVRTasksQueryTaskLocationResponseDto(
    Guid Id,
    string Name,
    string LocationCode
);

public record AuthorizedSearchVRTasksQueryMapObjectResponseDto(
    Guid Id,
    string Name,
    string ObjectCode
);

public record AuthorizedSearchVRTasksQueryActivityTypeResponseDto(
    Guid Id,
    string Name,
    string ActivityCode
);

public record AuthorizedSearchVRTasksQueryVRLessonResponseDto(
    Guid Id,
    string Name
);
