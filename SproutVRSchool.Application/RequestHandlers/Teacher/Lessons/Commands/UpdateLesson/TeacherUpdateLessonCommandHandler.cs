using MediatR;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.UpdateLesson;

public sealed class TeacherUpdateLessonCommandHandler(
    IUnitOfWork uow,
    ILocalStorageService localStorageService
    ) : IRequestHandler<TeacherUpdateLessonCommand, Unit>
{
    public async Task<Unit> Handle(TeacherUpdateLessonCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the existing entity from the database
        Lesson? existingLesson = await uow.Repository<Lesson>().GetEntityByIdAsync(request.LessonId);

        // 2. Check if lesson not exists
        if (existingLesson is null)
        {
            throw new SvrNotFoundException($"Lesson with ID {request.LessonId} not found.");
        }

        // 3. If uploading new file, then retrieve the relative file path
        string newResourceRelativeFilePath = string.Empty;
        if (request.ResourceFile != null && request.ResourceFile.Length != 0)
        {
            // 4. Check if the existing lesson has resource or not, if not then set
            //    If yes then delete it, and set

            if (!string.IsNullOrEmpty(existingLesson.ResourceRelativeFilePath))
            {
                await localStorageService.DeleteFileInLocalStorageAsync(existingLesson.ResourceRelativeFilePath);
            }

            newResourceRelativeFilePath = await localStorageService.SaveLessonResourceAsync(
                existingLesson.TeacherId,
                request.LessonId,
                request.ResourceFile
            );
        }

        // 5. Update the entity's properties with the new values
        existingLesson.Update(
            newName: request.Name,
            newDescription: request.Description,
            newResourceRelativeFilePath: newResourceRelativeFilePath
        );

        // 5. Update to the database
        uow.Repository<Lesson>().Update(existingLesson);
        await uow.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
