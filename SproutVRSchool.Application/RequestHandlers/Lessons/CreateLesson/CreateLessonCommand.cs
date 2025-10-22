using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.CreateLesson;

public record CreateLessonCommand : IRequest<Guid>
{
    public Guid SubjectId { get; init; }
    public Guid TeacherId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    // Resource file can be null 
    public IFormFile ResourceFile { get; init; }
}

public class CreateLessonCommandHandler(
    IUnitOfWork uow,
    ILocalStorageService fileStorageService
    ) : IRequestHandler<CreateLessonCommand, Guid>
{
    public async Task<Guid> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        string resourceRelativeFilePath = string.Empty;
        var createdLessonId = Guid.NewGuid();

        // If uploading resource file, then saving locally
        if (request.ResourceFile != null)
        {
            // Save the resource file to local storage
            resourceRelativeFilePath = await fileStorageService.SaveLessonResourceAsync(
                request.TeacherId,
                createdLessonId,
                request.ResourceFile
                );
        }

        var createdLesson = Lesson.Create(
            subjectId: request.SubjectId,
            teacherId: request.TeacherId,
            name: request.Name,
            description: request.Description,
            resourceRelativeFilePath: resourceRelativeFilePath
        );

        uow.Repository<Lesson>().Add(createdLesson);
        await uow.SaveChangesAsync(cancellationToken);

        return createdLesson.Id;
    }
}
