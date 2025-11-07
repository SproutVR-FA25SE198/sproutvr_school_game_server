namespace SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.GetMapObjectById;

public record AuthorizedGetMapObjectByIdQueryMapResponseDto
{
    public Guid Id { get; init; }
    public string MapCode { get; init; }
    public string Name { get; init; }
}
