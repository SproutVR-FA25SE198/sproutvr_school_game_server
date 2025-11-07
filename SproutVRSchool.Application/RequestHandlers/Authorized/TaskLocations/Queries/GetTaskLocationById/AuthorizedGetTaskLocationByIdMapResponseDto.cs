namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.GetTaskLocationById;

public record AuthorizedGetTaskLocationByIdMapResponseDto
{
    public Guid Id { get; init; }
    public string MapCode { get; init; }
    public string Name { get; init; }
}
