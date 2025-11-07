using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.GetMapById;

public record AuthorizedGetMapByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetMapByIdQuerySubjectResponseDto Subject { get; init; } // Nested DTO
    public string MapCode { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public string PreviewUrl { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}
