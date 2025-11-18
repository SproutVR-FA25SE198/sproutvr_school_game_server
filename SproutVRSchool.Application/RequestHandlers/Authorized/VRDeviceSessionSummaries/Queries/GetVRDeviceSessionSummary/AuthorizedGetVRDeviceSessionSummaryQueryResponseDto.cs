namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.GetVRDeviceSessionSummary;

public record AuthorizedGetVRDeviceSessionSummaryQueryResponseDto(
    Guid VRDeviceId,
    Guid VRLearningSessionId,
    string StudentName,
    int NoTasksCompleted,
    int NoTasksUncompleted,
    int NoCorrected,
    int NoInCorrected,
    int TotalTasks,
    AuthorizedGetVRDeviceSessionSummaryQueryDeviceResponseDto VRDevice,
    AuthorizedGetVRDeviceSessionSummaryQueryVRLearningSessionResponseDto VRLearningSession,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

public record AuthorizedGetVRDeviceSessionSummaryQueryDeviceResponseDto(
    Guid Id,
    string DeviceName,
    string SerialNumber
);

public record AuthorizedGetVRDeviceSessionSummaryQueryVRLearningSessionResponseDto(
    Guid Id,
    string ClassName
);
