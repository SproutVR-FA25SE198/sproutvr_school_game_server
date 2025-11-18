using SproutVRSchool.Application.Commons.Requests;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.SearchVRDeviceSessionSummaries;

public sealed class AuthorizedSearchVRDeviceSessionSummariesQueryParams : BaseGetListParams
{
    public string? StudentName { get; set; }
    public Guid? VRLearningSessionId { get; set; }
    public Guid? VRDeviceId { get; set; }

    public int? MinTasksCompleted { get; set; }
    public int? MaxTasksCompleted { get; set; }

    public string? SortBy { get; set; } = string.Empty;
}
