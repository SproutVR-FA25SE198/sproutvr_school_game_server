namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.GetMapObjectById;

public record AuthorizedGetMapObjectByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetMapObjectByIdQueryMapResponseDto Map { get; init; } // Nested Map DTO
    public string ObjectCode { get; init; }
    public string Name { get; init; }
    public string ImageUrl { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}
