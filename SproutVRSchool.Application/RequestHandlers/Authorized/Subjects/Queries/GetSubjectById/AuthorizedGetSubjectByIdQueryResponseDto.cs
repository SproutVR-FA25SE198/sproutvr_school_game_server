using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.GetSubjectById;

public record AuthorizedGetSubjectByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public AuthorizedGetSubjectByIdQueryMasterSubjectResponseDto MasterSubject { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}

public record AuthorizedGetSubjectByIdQueryMasterSubjectResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
}
