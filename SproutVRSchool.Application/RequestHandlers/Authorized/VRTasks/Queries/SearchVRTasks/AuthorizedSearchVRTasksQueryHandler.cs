using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.SearchVRTasks;

public sealed class AuthorizedSearchVRTasksQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedSearchVRTasksQuery, GetListResultResponseDto<AuthorizedSearchVRTasksQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchVRTasksQueryResponseDto>> Handle(AuthorizedSearchVRTasksQuery request, CancellationToken cancellationToken)
    {
        // 1. Get items & count from repository using the Specification
        (IReadOnlyList<VRTask> Data, int Count) rawLists = await uow.Repository<VRTask>()
            .ListAsync(new VRTasksSpecification(request.AuthorizedSearchVRTasksQueryParams));

        // 2. Map the entities to the response DTOs
        var mappedItems = rawLists.Data.Select(task => new AuthorizedSearchVRTasksQueryResponseDto
        {
            Id = task.Id,
            TaskNumber = task.TaskNumber,
            Description = task.TaskDescription,
            CreatedAtUtc = task.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(task.CreatedAtUtc),

            // Map navigation properties
            TaskLocation = new AuthorizedSearchVRTasksQueryTaskLocationResponseDto(
                Id: task.TaskLocation.Id,
                Name: task.TaskLocation.Name,
                LocationCode: task.TaskLocation.LocationCode
            ),
            MapObject = new AuthorizedSearchVRTasksQueryMapObjectResponseDto(
                Id: task.MapObject.Id,
                Name: task.MapObject.Name,
                ObjectCode: task.MapObject.ObjectCode
            ),
            ActivityType = new AuthorizedSearchVRTasksQueryActivityTypeResponseDto(
                Id: task.ActivityType.Id,
                Name: task.ActivityType.Name,
                ActivityCode: task.ActivityType.ActivityCode
            ),
            VRLesson = new AuthorizedSearchVRTasksQueryVRLessonResponseDto(
                Id: task.VRLesson.Id,
                Name: task.VRLesson.Name
            )
        }).ToList();

        // 3. Construct the final paginated response DTO
        var result = new GetListResultResponseDto<AuthorizedSearchVRTasksQueryResponseDto>(
            pageSize: request.AuthorizedSearchVRTasksQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchVRTasksQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: mappedItems
        );

        return result;
    }
}


