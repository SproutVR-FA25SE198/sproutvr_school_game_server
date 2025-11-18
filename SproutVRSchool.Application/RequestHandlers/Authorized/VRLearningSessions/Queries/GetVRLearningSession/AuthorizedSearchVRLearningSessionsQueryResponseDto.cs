using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.GetVRLearningSession;

public record AuthorizedGetVRLearningSessionQueryResponseDto(
    Guid Id,
    string ClassName,
    DateTimeOffset StartTimeAtUtc,
    DateTimeOffset EndTimeAtUtc,
    int DurationInMinutes,
    StatusDto Status,
    AuthorizedGetVRLearningSessionLessonResponseDto VRLesson,
    AuthorizedGetVRLearningSessionTeacherResponseDto Teacher,
    IReadOnlyList<AuthorizedGetVRLearningSessionTaskProgressResponseDto> VRDeviceTaskProgresses,
    IReadOnlyList<AuthorizedGetVRLearningSessionSummaryResponseDto> VRDeviceSessionSummaries,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

public record AuthorizedGetVRLearningSessionLessonResponseDto(
    Guid Id,
    string Name
);

public record AuthorizedGetVRLearningSessionTeacherResponseDto(
    Guid Id,
    string Name
);

public record AuthorizedGetVRLearningSessionTaskProgressResponseDto(
    Guid Id,
    Guid VRDeviceId,
    Guid VRTaskId,
    string StudentName,
    bool IsCompleted,
    bool IsCorrect,
    DateTimeOffset? CompletionTimeAtUtc
);

public record AuthorizedGetVRLearningSessionSummaryResponseDto(
    Guid Id,
    string StudentName,
    int NoTasksCompleted
);
