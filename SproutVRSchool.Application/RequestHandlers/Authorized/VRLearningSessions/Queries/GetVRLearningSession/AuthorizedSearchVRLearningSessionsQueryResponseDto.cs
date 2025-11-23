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
    AuthorizedGetVRLearningSessionVRTaskResponseDto VRTask,
    string StudentName,
    bool IsCompleted,
    bool IsCorrect,
    DateTimeOffset? CompletionTimeAtUtc
);

public record AuthorizedGetVRLearningSessionSummaryResponseDto(
    Guid Id,
    AuthorizedGetVRLearningSessionVRDeviceResponseDto VRDevice,
    Guid VRLearningSessionId,
    string StudentName,
    int NoTasksCompleted
);

public record AuthorizedGetVRLearningSessionVRTaskResponseDto(
    Guid VRTaskId,
    int TaskNumber,
    string TaskDescription
);

public record AuthorizedGetVRLearningSessionVRDeviceResponseDto(
    Guid VRDeviceId,
    string DeviceName
);
