using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.GetVRDevice;

public record AuthorizedGetVRDeviceQueryResponseDto(
    Guid Id,
    string Name,
    string SerialNumber,
    StatusDto Status,
    IReadOnlyList<AuthorizedGetVRDeviceQueryTasksResponseDto> TaskProgresses,
    IReadOnlyList<AuthorizedGetVRDeviceQuerySessionSummariesResponseDto> SessionSummaries,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam)
{ }

public record AuthorizedGetVRDeviceQueryTasksResponseDto(
    Guid Id,
    Guid TaskId,
    bool IsCompleted,
    bool IsCorrect,
    DateTimeOffset? CompletionTimeUtc);

public record AuthorizedGetVRDeviceQuerySessionSummariesResponseDto(
    Guid Id,
    string StudentName,
    int NoTasksCompleted);
