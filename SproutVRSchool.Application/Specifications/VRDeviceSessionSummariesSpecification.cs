using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.SearchVRDeviceSessionSummaries;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;

namespace SproutVRSchool.Application.Specifications;

public sealed class VRDeviceSessionSummariesSpecification : BaseSpecification<VRDeviceSessionSummary>
{
    public VRDeviceSessionSummariesSpecification(AuthorizedSearchVRDeviceSessionSummariesQueryParams searchParams)
        : base(x =>
            (string.IsNullOrEmpty(searchParams.StudentName) || x.StudentName.Contains(searchParams.StudentName)) &&
            (!searchParams.VRLearningSessionId.HasValue || x.VRLearningSessionId == searchParams.VRLearningSessionId.Value) &&
            (!searchParams.VRDeviceId.HasValue || x.VRDeviceId == searchParams.VRDeviceId.Value) &&
            (!searchParams.MinTasksCompleted.HasValue || x.NoTasksCompleted >= searchParams.MinTasksCompleted.Value) &&
            (!searchParams.MaxTasksCompleted.HasValue || x.NoTasksCompleted <= searchParams.MaxTasksCompleted.Value)
        )
    {
        // Pagination
        if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
        {
            ApplyPaging(searchParams.PageSize.Value * (searchParams.PageIndex.Value - 1),
                        searchParams.PageSize.Value);
        }

        // Sort By
        if (string.IsNullOrEmpty(searchParams.SortBy))
        {
            searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchParams.SortBy)
        {
            case AppCts.SortingKeys.VRDeviceSessionSummary.NO_TASK_COMPLETED_ASC:
                AddOrderBy(x => x.NoTasksCompleted);
                break;
            case AppCts.SortingKeys.VRDeviceSessionSummary.NO_TASK_COMPLETED_DESC:
                AddOrderByDescending(x => x.NoTasksCompleted);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.NoTasksCompleted);
                break;
        }

        AddInclude(x => x.VRDevice);
        AddInclude(x => x.VRLearningSession);
    }

    /// <summary>
    /// Constructor for retrieving the summary of the specific device on the vr learning session
    /// </summary>
    /// <param name="vrDeviceId"></param>
    /// <param name="vrLearningSessionId"></param>
    public VRDeviceSessionSummariesSpecification(Guid vrLearningSessionId, Guid vrDeviceId)
        : base(x => x.VRDeviceId == vrDeviceId && x.VRLearningSessionId == vrLearningSessionId)
    {
        AddInclude(x => x.VRLearningSession);
        AddInclude(x => x.VRDevice);
        AddThenInclude(x => x.Include(a => a.VRLearningSession).ThenInclude(b => b.VRDeviceTaskProgresses));
    }
}
