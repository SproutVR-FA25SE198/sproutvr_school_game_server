using SproutVRSchool.Application.Commons.Requests;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.SearchVRTasks;

public sealed class AuthorizedSearchVRTasksQueryParams : BaseGetListParams
{
    public Guid? VRLessonId { get; set; }
    public Guid? TaskLocationId { get; set; }
    public Guid? MapObjectId { get; set; }
    public Guid? ActivityTypeId { get; set; }
    public string? Question { get; set; } = string.Empty;
    public int? TaskNumber { get; set; }
    public string? SortBy { get; set; } = string.Empty;
}
