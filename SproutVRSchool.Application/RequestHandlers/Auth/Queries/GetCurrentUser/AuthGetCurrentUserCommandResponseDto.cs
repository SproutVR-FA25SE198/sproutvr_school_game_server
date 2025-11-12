using System.Text.Json.Serialization;
using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;

public sealed record AuthGetCurrentUserCommandResponseDto
{
    public Guid UserId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string FullName { get; init; } = string.Empty;

    public StatusDto Status { get; init; }

    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();

    public DateOnly? DateOfBirth { get; init; }

    public DateTimeOffset JoinedAtUtc { get; init; }

    public DateTimeOffset JoinedAtVietNam { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OrganizationId { get; init; } = null;
}




