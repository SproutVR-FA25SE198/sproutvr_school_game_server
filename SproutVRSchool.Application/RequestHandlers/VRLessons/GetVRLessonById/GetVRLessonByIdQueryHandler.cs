using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.GetVRLessonById;

public sealed class GetVRLessonByIdQueryHandler(
    IUnitOfWork uow)
    : IRequestHandler<GetVRLessonByIdQuery, GetVRLessonByIdResponseDto>
{
    public async Task<GetVRLessonByIdResponseDto> Handle(GetVRLessonByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Create the specification
        var spec = new VRLessonSpecification(request.id);

        // 2. Fetch the VRLesson using the specification
        VRLesson vrLesson = await uow.Repository<VRLesson>().GetEntityBySpec(spec)
            ?? throw new NotFoundException($"VRLesson with ID {request.id} not found.");

        // 3. Map the entity to the response DTO
        return new GetVRLessonByIdResponseDto
        {
            Id = vrLesson.Id,
            Name = vrLesson.Name,
            Description = vrLesson.Description,
            Duration = vrLesson.MaxDuration,
            PresetJsonRelativeFilePath = vrLesson.PresetJsonRelativeFilePath,
            Status = new StatusDto(vrLesson.Status),

            // Map the included Lesson entity to its DTO
            Lesson = new GetVRLessonByIdLessonResponseDto
            {
                Id = vrLesson.Lesson.Id,
                Name = vrLesson.Lesson.Name,
                Description = vrLesson.Lesson.Description
            },

            // Map the included Map entity to its DTO
            Map = new GetVRLessonByIdMapResponseDto
            {
                Id = vrLesson.Map.Id,
                Name = vrLesson.Map.Name,
                MapCode = vrLesson.Map.MapCode
            },

            // Map the list of VRTask entities
            Tasks = vrLesson.VRTasks.Select(task => new GetVRLessonByIdTaskResponseDto
            {
                Id = task.Id,
                TaskNumber = task.TaskNumber,
                Description = task.Description,

                // Map nested task relations
                TaskLocation = new GetVRLessonByIdTaskLocationResponseDto
                {
                    Id = task.TaskLocation.Id,
                    Name = task.TaskLocation.Name,
                    LocationCode = task.TaskLocation.LocationCode
                },
                MapObject = new GetVRLessonByIdMapObjectResponseDto
                {
                    Id = task.MapObject.Id,
                    Name = task.MapObject.Name,
                    ObjectCode = task.MapObject.ObjectCode
                },
                ActivityType = new GetVRLessonByIdActivityTypeResponseDto
                {
                    Id = task.ActivityType.Id,
                    Name = task.ActivityType.Name,
                    ActivityCode = task.ActivityType.ActivityCode
                }
            }).ToList()
        };
    }
}
