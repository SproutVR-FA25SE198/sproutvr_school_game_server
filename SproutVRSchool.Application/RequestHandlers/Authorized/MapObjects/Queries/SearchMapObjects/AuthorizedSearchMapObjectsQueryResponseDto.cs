namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.SearchMapObjects;

public record AuthorizedSearchMapObjectsQueryResponseDto(
    Guid Id,
    string ObjectCode,
    string Name,
    string ImageUrl,
    AuthorizedSearchMapObjectsMapQueryResponseDto Map,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);


public record AuthorizedSearchMapObjectsMapQueryResponseDto(
    Guid Id,
    string MapCode,
    string Name,
    string ImageUrl,
    string PreviewUrl
);
