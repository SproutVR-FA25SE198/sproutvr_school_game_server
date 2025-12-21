using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.SearchVRLessons;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.Specifications;

internal sealed class VRLessonsSpecification : BaseSpecification<VRLesson>
{
    public VRLessonsSpecification(AuthorizedSearchVRLessonsQueryParams searchVRLessonsParam)
        : base(x =>
            (string.IsNullOrEmpty(searchVRLessonsParam.Name) || x.Name.Contains(searchVRLessonsParam.Name)) &&
            (!searchVRLessonsParam.TeacherId.HasValue || x.LessonId == searchVRLessonsParam.TeacherId) &&
            (!searchVRLessonsParam.LessonId.HasValue || x.LessonId == searchVRLessonsParam.LessonId) &&
            (!searchVRLessonsParam.MapId.HasValue || x.MapId == searchVRLessonsParam.MapId) &&
            (!searchVRLessonsParam.VRLessonStatus.HasValue || x.Status == searchVRLessonsParam.VRLessonStatus))
    {
        // 1. Pagination
        if (searchVRLessonsParam.IsPaginated!.Value && searchVRLessonsParam.PageSize.HasValue && searchVRLessonsParam.PageIndex.HasValue)
        {
            ApplyPaging(searchVRLessonsParam.PageSize.Value * (searchVRLessonsParam.PageIndex.Value - 1), searchVRLessonsParam.PageSize.Value);
        }

        // 2. Sorting
        if (string.IsNullOrEmpty(searchVRLessonsParam.SortBy))
        {
            searchVRLessonsParam.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchVRLessonsParam.SortBy)
        {
            case AppCts.SortingKeys.VRLessons.NAME_ASC:
                AddOrderBy(x => x.Name);
                break;
            case AppCts.SortingKeys.VRLessons.NAME_DESC:
                AddOrderByDescending(x => x.Name);
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
                AddOrderBy(x => x.Name);
                break;
        }

        // Includes
        AddInclude(x => x.Lesson);
        AddInclude(x => x.Map);
    }

    public VRLessonsSpecification(Guid vrLessonId)
        : base(x => x.Id == vrLessonId)
    {
        AddInclude(vrlesson => vrlesson.Map);
        AddInclude(vrLesson => vrLesson.Lesson);
        AddThenInclude(vrLesson => vrLesson.Include(j => j.VRTasks).ThenInclude(j => j.MapObject));
        AddThenInclude(vrLesson => vrLesson.Include(j => j.VRTasks).ThenInclude(j => j.ActivityType));
        AddThenInclude(vrLesson => vrLesson.Include(j => j.VRTasks).ThenInclude(j => j.TaskLocation));
    }
}
