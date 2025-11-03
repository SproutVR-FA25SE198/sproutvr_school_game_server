namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.SearchActivityTypes;

public record AuthorizedSearchActivityTypesQueryResponseDto(
    Guid Id,
    string ActivityCode,
    string Name,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);
