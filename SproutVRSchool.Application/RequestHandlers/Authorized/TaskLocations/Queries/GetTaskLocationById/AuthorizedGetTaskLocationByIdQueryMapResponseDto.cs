namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.GetTaskLocationById;

public record AuthorizedGetTaskLocationByIdQueryMapResponseDto
{
    public Guid Id { get; init; }
    public string MapCode { get; init; }
    public string Name { get; init; }
}
