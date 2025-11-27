namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.GetActivityTypeById;

public sealed record AuthorizedGetActivityTypeByIdQueryResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ActivityCode { get; init; }
    public DateTimeOffset CreatedAtUtc { get; init; }
    public DateTimeOffset CreatedAtVietNam { get; init; }
}
