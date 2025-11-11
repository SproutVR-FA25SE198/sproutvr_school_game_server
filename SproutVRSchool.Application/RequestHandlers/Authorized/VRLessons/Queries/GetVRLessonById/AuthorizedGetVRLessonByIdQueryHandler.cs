using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;

public sealed class AuthorizedGetVRLessonByIdQueryHandler(
    IUnitOfWork uow, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AuthorizedGetVRLessonByIdQuery, AuthorizedGetVRLessonByIdQueryResponseDto>
{
    public async Task<AuthorizedGetVRLessonByIdQueryResponseDto> Handle(AuthorizedGetVRLessonByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Create the specification
        var spec = new VRLessonsSpecification(request.id);

        // 2. Fetch the VRLesson using the specification
        VRLesson vrLesson = await uow.Repository<VRLesson>().GetEntityBySpec(spec)
            ?? throw new SvrResourceNotFoundException($"VRLesson with ID {request.id} not found.");

        // 3. Map the entity to the response DTO
        return new AuthorizedGetVRLessonByIdQueryResponseDto
        {
            Id = vrLesson.Id,
            Name = vrLesson.Name,
            Description = vrLesson.Description,
            Duration = vrLesson.MaxDuration,
            PresetJsonRelativeFilePath = vrLesson.PresetJsonRelativeFilePath,
            Status = new StatusDto(vrLesson.Status),
            CreatedAtUtc = vrLesson.CreatedAtUtc,
            CreatedAtVietNam = dateTimeProvider.ConvertToVietNamTime(vrLesson.CreatedAtUtc),

            // Map the included Lesson entity to its DTO
            Lesson = new AuthorizedGetVRLessonByIdQueryLessonResponseDto
            {
                Id = vrLesson.Lesson.Id,
                Name = vrLesson.Lesson.Name,
                Description = vrLesson.Lesson.Description,
            },

            // Map the included Map entity to its DTO
            Map = new AuthorizedGetVRLessonByIdQueryMapResponseDto
            {
                Id = vrLesson.Map.Id,
                Name = vrLesson.Map.Name,
                MapCode = vrLesson.Map.MapCode,
                ImageUrl = vrLesson.Map.ImageUrl
            },

            // Map the list of VRTask entities
            Tasks = vrLesson.VRTasks.Select(task => new AuthorizedGetVRLessonByIdQueryTaskResponseDto
            {
                Id = task.Id,
                TaskNumber = task.TaskNumber,
                Question = task.Question,
                TaskDescription = task.TaskDescription,

                // Map nested task relations
                TaskLocation = new AuthorizedGetVRLessonByIdQueryTaskLocationResponseDto
                {
                    Id = task.TaskLocation.Id,
                    Name = task.TaskLocation.Name,
                    LocationCode = task.TaskLocation.LocationCode,
                    ImageUrl = task.TaskLocation.ImageUrl
                },
                MapObject = new AuthorizedGetVRLessonByIdQueryMapObjectResponseDto
                {
                    Id = task.MapObject.Id,
                    Name = task.MapObject.Name,
                    ObjectCode = task.MapObject.ObjectCode,
                    ImageUrl = task.MapObject.ImageUrl
                },
                ActivityType = new AuthorizedGetVRLessonByIdQueryActivityTypeResponseDto
                {
                    Id = task.ActivityType.Id,
                    Name = task.ActivityType.Name,
                    ActivityCode = task.ActivityType.ActivityCode
                }
            }).ToList()
        };
    }
}
