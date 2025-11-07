using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.GetMasterSubjectById;

public record AuthorizedGetMasterSubjectByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public StatusDto Status { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}
