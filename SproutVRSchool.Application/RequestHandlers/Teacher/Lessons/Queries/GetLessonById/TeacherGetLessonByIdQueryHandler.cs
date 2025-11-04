using MediatR;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Queries.GetLessonById;

public sealed class TeacherGetLessonByIdQueryHandler(
    IUnitOfWork uow
    ) : IRequestHandler<TeacherGetLessonByIdQuery, TeacherGetLessonByIdQueryResponseDto>
{
    public async Task<TeacherGetLessonByIdQueryResponseDto> Handle(TeacherGetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch the lesson entity from the repo
        var spec = new LessonsSpecification(request.Id);

        Lesson? lesson = await uow.Repository<Lesson>()
            .GetEntityBySpec(spec);

        // 2. If not found, throw SvrNotFoundException
        if (lesson is null)
        {
            throw new SvrResourceNotFoundException($"Lesson with ID {request.Id} not found.");
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
        return new TeacherGetLessonByIdQueryResponseDto
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
