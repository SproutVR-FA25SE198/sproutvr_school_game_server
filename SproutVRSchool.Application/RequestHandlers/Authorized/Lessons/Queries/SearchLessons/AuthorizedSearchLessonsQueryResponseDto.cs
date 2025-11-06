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
    string Name,
    string Description,
    string ImageUrl
);

public record AuthorizedSearchLessonsQuerySubjectDto(
    string Name,
    string Description,
    string ImageUrl
);

public record AuthorizedSearchLessonsQueryTeacherDto(
    string FirstName,
    string LastName
);
