using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;

public sealed class AuthorizedSearchVRLearningSessionsQueryParams : BaseGetListParams
{
    public string? ClassName { get; set; }
    public Guid? VRLessonId { get; set; }
    public Guid? TeacherId { get; set; }
    public VRLearningSessionStatus? VrLearningSessionStatus { get; set; }

    public string? SortBy { get; set; } = string.Empty;
}

