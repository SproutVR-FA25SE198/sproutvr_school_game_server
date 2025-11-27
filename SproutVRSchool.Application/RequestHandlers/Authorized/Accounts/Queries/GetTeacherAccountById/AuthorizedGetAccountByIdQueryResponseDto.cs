using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.GetTeacherAccountById;

public sealed record AuthorizedGetAccountByIdQueryResponseDto(
    Guid TeacherId,
    string Email,
    string FullName,
    StatusDto Status,
    IReadOnlyList<string> Roles,
    DateOnly? DateOfBirth,
    DateTimeOffset JoinedAtUtc,
    IReadOnlyList<AuthorizedGetAccountByIdLessonResponseDto> Lessons,
    IReadOnlyList<AuthorizedGetAccountByIdVRLearningSessionResponseDto> VRLearningSessions
);

public sealed record AuthorizedGetAccountByIdLessonResponseDto(
    Guid LessonId,
    string Name,
    string Status
);

public sealed record AuthorizedGetAccountByIdVRLearningSessionResponseDto(
    Guid SessionId,
    string ClassName,
    DateTimeOffset CreatedAtUtc
);
