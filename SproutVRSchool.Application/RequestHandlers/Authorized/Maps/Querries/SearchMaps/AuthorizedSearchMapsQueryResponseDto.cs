using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Querries.SearchMaps;

public record AuthorizedSearchMapsQueryResponseDto(
    Guid Id,
    string Name,
    AuthorizedSearchMapsQuerySubjectResponseDto Subject,
    string MapCode,
    StatusDto Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam);
public record AuthorizedSearchMapsQuerySubjectResponseDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl);
