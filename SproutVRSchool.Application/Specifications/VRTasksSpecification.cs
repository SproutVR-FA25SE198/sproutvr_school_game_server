using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.SearchVRTasks;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Application.Specifications;

public sealed class VRTasksSpecification : BaseSpecification<VRTask>
{
    public VRTasksSpecification(ActivateRoomParams activateRoomParams)
        : base(x => x.VRLessonId == activateRoomParams.VRLessonId)
    {
    }

    public VRTasksSpecification(Guid id) : base(x => x.Id == id)
    {
        AddInclude(x => x.TaskLocation);
        AddInclude(x => x.MapObject);
        AddInclude(x => x.ActivityType);
        AddInclude(x => x.VRLesson);
    }

    public VRTasksSpecification(AuthorizedSearchVRTasksQueryParams searchVRTasksParam)
        : base(x =>
            // Filters for all GUID foreign keys
            (!searchVRTasksParam.VRLessonId.HasValue || x.VRLessonId == searchVRTasksParam.VRLessonId) &&
            (!searchVRTasksParam.TaskLocationId.HasValue || x.TaskLocationId == searchVRTasksParam.TaskLocationId) &&
            (!searchVRTasksParam.MapObjectId.HasValue || x.MapObjectId == searchVRTasksParam.MapObjectId) &&
            (!searchVRTasksParam.ActivityTypeId.HasValue || x.ActivityTypeId == searchVRTasksParam.ActivityTypeId) &&
            (!searchVRTasksParam.TaskNumber.HasValue || x.TaskNumber == searchVRTasksParam.TaskNumber))
    {
        // 1. Pagination
        if (searchVRTasksParam.IsPaginated!.Value && searchVRTasksParam.PageSize.HasValue && searchVRTasksParam.PageIndex.HasValue)
        {
            ApplyPaging(searchVRTasksParam.PageSize.Value * (searchVRTasksParam.PageIndex.Value - 1), searchVRTasksParam.PageSize.Value);
        }

        // 2. Sorting
        if (string.IsNullOrEmpty(searchVRTasksParam.SortBy))
        {
            searchVRTasksParam.SortBy = AppCts.SortingKeys.DEFAULT;
        }

        switch (searchVRTasksParam.SortBy)
        {
            case AppCts.SortingKeys.VRTasks.TASK_NUMBER_ASC:
                AddOrderBy(x => x.TaskNumber);
                break;
            case AppCts.SortingKeys.VRTasks.TASK_NUMBER_DESC:
                AddOrderByDescending(x => x.TaskNumber);
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
                AddOrderBy(x => x.TaskNumber);
                break;
        }

        // 3. Include
        AddInclude(x => x.TaskLocation);
        AddInclude(x => x.MapObject);
        AddInclude(x => x.ActivityType);
        AddInclude(x => x.VRLesson);
    }
}
