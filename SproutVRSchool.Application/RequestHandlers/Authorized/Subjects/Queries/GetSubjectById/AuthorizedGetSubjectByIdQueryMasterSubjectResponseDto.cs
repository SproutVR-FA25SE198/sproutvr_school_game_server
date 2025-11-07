namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.GetSubjectById;

public record AuthorizedGetSubjectByIdQueryMasterSubjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
}
