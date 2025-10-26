using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.GetLessonById;

public sealed class GetLessonByIdQueryHandler(
    IUnitOfWork uow
    ) : IRequestHandler<GetLessonByIdQuery, GetLessonByIdResponseDto>
{
    async Task<GetLessonByIdResponseDto> IRequestHandler<GetLessonByIdQuery, GetLessonByIdResponseDto>.Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the lesson entity from the repo
        var spec = new LessonSpecification(request.Id);

        Lesson? lesson = await uow.Repository<Lesson>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw NotFoundException
        if (lesson is null)
        {
            throw new NotFoundException($"Lesson with ID {request.Id} not found.");
        }

        // 3. Map the lesson entity to GetLessonByIdResponseDto
        var subject = new GetLessonByIdSubjectResponseDto
        {
            Id = lesson.Subject.Id,
            Name = lesson.Subject.Name,
            Description = lesson.Subject.Description,
            ImageUrl = lesson.Subject.ImageUrl
        };

        // 4. Map the teacher info
        var teacher = new GetLessonByIdTeacherResponseDto
        {
            Id = lesson.Teacher.Id,
            FirstName = lesson.Teacher.FirstName,
            LastName = lesson.Teacher.LastName,
            Email = lesson.Teacher.Email ?? string.Empty,
        };

        // 5. Return the response DTO
        return new GetLessonByIdResponseDto
        {
            Id = lesson.Id,
            Subject = subject,
            Teacher = teacher,
            Name = lesson.Name,
            Description = lesson.Description,
            ResourceRelativeFilePath = lesson.ResourceRelativeFilePath,
            Status = new StatusDto(lesson.Status),
            CreatedAtUtc = lesson.CreatedAtUtc
        };
    }
}
