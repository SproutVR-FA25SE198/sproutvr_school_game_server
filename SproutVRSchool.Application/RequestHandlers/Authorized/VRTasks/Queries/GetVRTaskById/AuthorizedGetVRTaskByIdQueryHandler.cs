using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.GetVRTaskById;

public sealed class AuthorizedGetVRTaskByIdQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
    ) : IRequestHandler<AuthorizedGetVRTaskByIdQuery, AuthorizedGetVRTaskByIdQueryResponseDto>
{
    public async Task<AuthorizedGetVRTaskByIdQueryResponseDto> Handle(AuthorizedGetVRTaskByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the VRTask entity from the repo, including all related entities
        var spec = new VRTasksSpecification(request.Id);

        VRTask? vrTask = await uow.Repository<VRTask>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (vrTask is null)
        {
            throw new SvrResourceNotFoundException($"VRTask with ID {request.Id} not found.");
        }

        // 3. Map the nested navigation properties
        var taskLocationDto = new AuthorizedGetVRTaskByIdQueryTaskLocationResponseDto(
            Id: vrTask.TaskLocation.Id,
            Name: vrTask.TaskLocation.Name,
            LocationCode: vrTask.TaskLocation.LocationCode
        );

        var mapObjectDto = new AuthorizedGetVRTaskByIdQueryMapObjectResponseDto(
            Id: vrTask.MapObject.Id,
            Name: vrTask.MapObject.Name,
            ObjectCode: vrTask.MapObject.ObjectCode
        );

        var activityTypeDto = new AuthorizedGetVRTaskByIdQueryActivityTypeResponseDto(
            Id: vrTask.ActivityType.Id,
            Name: vrTask.ActivityType.Name,
            ActivityCode: vrTask.ActivityType.ActivityCode
        );

        var vrLessonDto = new AuthorizedGetVRTaskByIdQueryVRLessonResponseDto(
            Id: vrTask.VRLesson.Id,
            Name: vrTask.VRLesson.Name
        );

        // 4. Return the response DTO
        return new AuthorizedGetVRTaskByIdQueryResponseDto
        {
            Id = vrTask.Id,
            TaskLocation = taskLocationDto,
            MapObject = mapObjectDto,
            ActivityType = activityTypeDto,
            VRLesson = vrLessonDto,
            TaskNumber = vrTask.TaskNumber,
            Description = vrTask.TaskDescription,
            CreatedAtUtc = vrTask.CreatedAtUtc,
            // Calculate CreatedAtVietNam time
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(vrTask.CreatedAtUtc)
        };
    }
}

