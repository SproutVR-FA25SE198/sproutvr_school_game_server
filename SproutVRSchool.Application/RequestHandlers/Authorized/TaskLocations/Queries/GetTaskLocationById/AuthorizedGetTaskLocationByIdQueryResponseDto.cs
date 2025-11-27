namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.GetTaskLocationById;

public record AuthorizedGetTaskLocationByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetTaskLocationByIdQueryMapResponseDto Map { get; init; } // Nested Map DTO
    public string LocationCode { get; init; }
    public string Name { get; init; }
    public string ImageUrl { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; } // Included as requested
}
