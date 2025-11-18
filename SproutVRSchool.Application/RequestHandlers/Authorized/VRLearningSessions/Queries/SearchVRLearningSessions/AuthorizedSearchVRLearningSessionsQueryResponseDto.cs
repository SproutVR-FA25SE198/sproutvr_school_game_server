using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;

public record AuthorizedSearchVRLearningSessionsQueryResponseDto(
    Guid Id,
    string ClassName,
    DateTimeOffset StartTimeAtUtc,
    DateTimeOffset EndTimeAtUtc,
    StatusDto Status,
    int DurationInMinutes,
    AuthorizedSearchVRLearningSessionsQueryLessonResponseDto VRLesson,
    AuthorizedSearchVRLearningSessionsQueryTeacherResponseDto Teacher,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

public record AuthorizedSearchVRLearningSessionsQueryLessonResponseDto(
    Guid Id,
    string Name
);

public record AuthorizedSearchVRLearningSessionsQueryTeacherResponseDto(
    Guid Id,
    string Name
);
