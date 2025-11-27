using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;

public record AuthorizedSearchSubjectsQueryResponseDto(
    Guid Id,
    AuthorizedSearchSubjectsQueryMasterSubjectResponseDto MasterSubject,
    string Name,
    string Description,
    string ImageUrl,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

public record AuthorizedSearchSubjectsQueryMasterSubjectResponseDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);
