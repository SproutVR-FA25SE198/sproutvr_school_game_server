namespace SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;

public sealed record AuthorizedSearchAccountsQueryResponseDto(
    Guid UserId,
    string Email,
    string FullName,
    string Status,
    IReadOnlyList<string> Roles,
    DateOnly? DateOfBirth,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset CreatedAtVietNam
);

