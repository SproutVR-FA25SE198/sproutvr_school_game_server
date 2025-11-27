using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.SearchLessons;

public record AuthorizedSearchLessonsQueryResponseDto(
    Guid Id,
    string Name,
    string Description,
    string ResourceRelativeFilePath,
    StatusDto Status,
    AuthorizedSearchLessonsQuerySubjectDto Subject,
    AuthorizedSearchLessonsQueryTeacherDto Teacher,
    AuthorizedSearchLessonsQueryMasterSubjectDto MasterSubject,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam,
    int VRLessonsCount
);

public record AuthorizedSearchLessonsQueryMasterSubjectDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl
);

public record AuthorizedSearchLessonsQuerySubjectDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl
);

public record AuthorizedSearchLessonsQueryTeacherDto(
    Guid Id,
    string FirstName,
    string LastName
);
