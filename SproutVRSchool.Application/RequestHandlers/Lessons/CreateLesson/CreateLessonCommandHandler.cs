using MediatR;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.CreateLesson;

public sealed class CreateLessonCommandHandler(
    IUnitOfWork uow,
    ILocalStorageService localStorageService
    ) : IRequestHandler<CreateLessonCommand, Guid>
{
    public async Task<Guid> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        // 1. Uploading resource file is optional
        string resourceRelativeFilePath = string.Empty;
        var createdLessonId = Guid.NewGuid();

        // 2. Only save file if presence
        if (request.ResourceFile != null && request.ResourceFile.Length != 0)
        {
            resourceRelativeFilePath = await localStorageService.SaveLessonResourceAsync(
                request.TeacherId,
                createdLessonId,
                request.ResourceFile
            );
        }

        // 3. Create the lesson
        var createdLesson = Lesson.Create(
            createdLessonId,
            subjectId: request.SubjectId,
            teacherId: request.TeacherId,
            name: request.Name,
            description: request.Description,
            resourceRelativeFilePath: resourceRelativeFilePath
        );

        // 4. Save to db
        uow.Repository<Lesson>().Add(createdLesson);
        await uow.SaveChangesAsync(cancellationToken);

        return createdLesson.Id;
    }
}
