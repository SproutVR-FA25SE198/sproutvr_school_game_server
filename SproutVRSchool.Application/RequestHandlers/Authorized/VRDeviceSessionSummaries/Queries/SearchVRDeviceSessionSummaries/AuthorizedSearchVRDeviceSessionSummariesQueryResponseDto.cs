namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.SearchVRDeviceSessionSummaries;

public record AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto(
    string StudentName,
    int NoTasksCompleted,
    AuthorizedSearchVRDeviceSessionSummariesQueryDeviceResponseDto VRDevice,
    AuthorizedSearchVRDeviceSessionSummariesQueryVRLearningSessionResponseDto VRLearningSession,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);
public record AuthorizedSearchVRDeviceSessionSummariesQueryDeviceResponseDto(
    Guid Id,
    string DeviceName,
    string SerialNumber
);

public record AuthorizedSearchVRDeviceSessionSummariesQueryVRLearningSessionResponseDto(
    Guid Id,
    string ClassName
);
