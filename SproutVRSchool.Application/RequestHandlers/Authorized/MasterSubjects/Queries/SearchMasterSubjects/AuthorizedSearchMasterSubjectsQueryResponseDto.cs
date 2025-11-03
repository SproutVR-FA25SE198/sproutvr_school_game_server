using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.SearchMasterSubjects;

public record AuthorizedSearchMasterSubjectsQueryResponseDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);
