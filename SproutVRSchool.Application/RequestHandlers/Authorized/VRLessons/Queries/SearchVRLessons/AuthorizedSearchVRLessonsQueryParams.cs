using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.SearchVRLessons;

public sealed class AuthorizedSearchVRLessonsQueryParams : BaseGetListParams
{
    public string? Name { get; set; }
    public Guid? LessonId { get; set; }
    public Guid? MapId { get; set; }
    public VRLessonStatus? VRLessonStatus { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}


