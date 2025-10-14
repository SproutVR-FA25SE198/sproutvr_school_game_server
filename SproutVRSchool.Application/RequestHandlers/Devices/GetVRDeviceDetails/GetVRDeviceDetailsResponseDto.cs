using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Devices.GetVRDeviceDetails;

public record GetVRDeviceDetailsResponseDto(
    Guid Id,
    string Name,
    StatusDto Status,
    IReadOnlyList<GetVRDeviceDetailsTasksResponseDto> TaskProgresses,
    IReadOnlyList<GetVRDeviceDetailsSessionSummariesResponseDto> SessionSummaries)
{ }

public record GetVRDeviceDetailsTasksResponseDto(
    Guid TaskProgressId,
    Guid TaskId,
    string QuestionText,
    bool IsCorrect,
    DateTimeOffset CompletionTimeUtc);

public record GetVRDeviceDetailsSessionSummariesResponseDto(
    Guid SummaryId,
    string StudentName,
    int NoTasksCompleted);
