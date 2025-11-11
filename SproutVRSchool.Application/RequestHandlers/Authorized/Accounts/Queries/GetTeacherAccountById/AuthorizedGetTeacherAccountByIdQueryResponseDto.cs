using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.GetTeacherAccountById;

public sealed record AuthorizedGetTeacherAccountByIdQueryResponseDto(
    Guid TeacherId,
    string Email,
    string FullName,
    StatusDto Status,
    IReadOnlyList<string> Roles,
    DateOnly? DateOfBirth,
    DateTimeOffset JoinedAtUtc,
    IReadOnlyList<AuthorizedGetTeacherAccountByIdLessonResponseDto> Lessons,
    IReadOnlyList<AuthorizedGetTeacherAccountByIdVRLearningSessionResponseDto> VRLearningSessions
);

public sealed record AuthorizedGetTeacherAccountByIdLessonResponseDto(
    Guid LessonId,
    string Name,
    string Status
);

public sealed record AuthorizedGetTeacherAccountByIdVRLearningSessionResponseDto(
    Guid SessionId,
    string ClassName,
    DateTimeOffset CreatedAtUtc
);
