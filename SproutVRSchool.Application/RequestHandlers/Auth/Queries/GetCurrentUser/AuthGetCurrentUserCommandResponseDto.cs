using Newtonsoft.Json;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;

public sealed record AuthGetCurrentUserCommandResponseDto(
    Guid UserId,
    string Email,
    string FullName,
    string Status,
    IReadOnlyList<string> Roles,

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    string? OrganizationId,

    DateOnly? DateOfBirth,
    DateTimeOffset JoinedAtUtc,
    DateTimeOffset JoinedAtVietNam
);

