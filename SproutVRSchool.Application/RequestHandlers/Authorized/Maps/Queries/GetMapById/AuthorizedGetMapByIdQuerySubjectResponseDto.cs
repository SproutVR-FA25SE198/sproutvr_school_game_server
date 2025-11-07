namespace SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.GetMapById;

public record AuthorizedGetMapByIdQuerySubjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ImageUrl { get; init; }
}
