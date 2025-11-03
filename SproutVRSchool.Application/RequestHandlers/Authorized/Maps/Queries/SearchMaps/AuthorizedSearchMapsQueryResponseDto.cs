using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.SearchMaps;

public record AuthorizedSearchMapsQueryResponseDto(
    Guid Id,
    string Name,
    AuthorizedSearchMapsQuerySubjectResponseDto Subject,
    string MapCode,
    StatusDto Status,
    string ImageUrl,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam);
public record AuthorizedSearchMapsQuerySubjectResponseDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl);
