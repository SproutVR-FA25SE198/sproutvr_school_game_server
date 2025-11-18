using SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Application.Specifications;

internal sealed class VRLearningSessionsSpecification : BaseSpecification<VRLearningSession>
{
    public VRLearningSessionsSpecification(AuthorizedSearchVRLearningSessionsQueryParams searchParams)
        : base(x =>
            (string.IsNullOrEmpty(searchParams.ClassName) || x.ClassName.Contains(searchParams.ClassName)) &&

            (!searchParams.VRLessonId.HasValue || x.VRLessonId == searchParams.VRLessonId.Value) &&
            (!searchParams.TeacherId.HasValue || x.TeacherId == searchParams.TeacherId.Value) &&

            (!searchParams.VrLearningSessionStatus.HasValue || x.Status == searchParams.VrLearningSessionStatus.Value)
        )
    {
        // Pagination
        if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
        {
            ApplyPaging(searchParams.PageSize.Value * (searchParams.PageIndex.Value - 1), searchParams.PageSize.Value);
        }

        // Sorting
        if (string.IsNullOrEmpty(searchParams.SortBy))
        {
            searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchParams.SortBy)
        {
            case AppCts.SortingKeys.VRLearningSessions.CLASS_NAME_ASC:
                AddOrderBy(x => x.ClassName);
                break;
            case AppCts.SortingKeys.VRLearningSessions.CLASS_NAME_DESC:
                AddOrderByDescending(x => x.ClassName);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                AddOrderBy(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.CreatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_ASC:
                AddOrderBy(x => x.UpdatedAtUtc);
                break;
            case AppCts.SortingKeys.UPDATED_AT_UTC_DESC:
                AddOrderByDescending(x => x.UpdatedAtUtc);
                break;
            default:
                AddOrderBy(x => x.ClassName);
                break;
        }

        // Include
        AddInclude(x => x.VRLesson);
        AddInclude(x => x.Teacher);
    }

    /// <summary>
    /// VR Learning Sessions By Id
    /// </summary>
    /// <param name="id"></param>
    public VRLearningSessionsSpecification(Guid id) : base(x => x.Id == id)
    {
        // Specification to get a single item by ID, with its relations
        AddInclude(x => x.VRLesson);
        AddInclude(x => x.Teacher);
        AddInclude(x => x.VRDeviceSessionSummaries);
        AddInclude(x => x.VRDeviceTaskProgresses);
    }
}
