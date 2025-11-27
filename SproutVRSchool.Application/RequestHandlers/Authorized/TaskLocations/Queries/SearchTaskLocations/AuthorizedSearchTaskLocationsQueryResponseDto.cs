namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.SearchTaskLocations;

public record AuthorizedSearchTaskLocationsQueryResponseDto(
    Guid Id,
    string LocationCode,
    string Name,
    string ImageUrl,
    AuthorizedSearchTaskLocationsMapQueryResponseDto Map,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);


public record AuthorizedSearchTaskLocationsMapQueryResponseDto(
    Guid Id,
    string MapCode,
    string Name,
    string ImageUrl,
    string PreviewUrl
);
