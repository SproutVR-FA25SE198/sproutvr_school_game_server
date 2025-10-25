using MediatR;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Lessons.UpdateLesson;

public record UpdateLessonCommand : IRequest<Unit>
{
    public Guid LessonId { get; set; }
    public Guid? SubjectId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IFormFile? ResourceFile { get; init; }
    public LessonStatus? Status { get; init; }
}

public class UpdateLessonCommandHandler(
    IUnitOfWork uow,
    ILocalStorageService localStorageService
    ) : IRequestHandler<UpdateLessonCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the existing entity from the database
        Lesson? existingLesson = await uow.Repository<Lesson>().GetEntityByIdAsync(request.LessonId);

        // 2. Check if lesson not exists
        if (existingLesson is null)
        {
            throw new NotFoundException($"Lesson with ID {request.LessonId} not found.");
        }

        // 3. If uploading new file, then retrieve the relative file patht
        string newResourceRelativeFilePath = string.Empty;
        if (request.ResourceFile != null && request.ResourceFile.Length != 0)
        {
            newResourceRelativeFilePath = await localStorageService.SaveLessonResourceAsync(
                existingLesson.TeacherId,
                request.LessonId,
                request.ResourceFile
            );
        }

        // 4. Update the entity's properties with the new values
        existingLesson.Update(
            newName: request.Name,
            newDescription: request.Description,
            newSubjectId: request.SubjectId,
            newStatus: request.Status,
            newResourceRelativeFilePath: newResourceRelativeFilePath
        );

        // 5. Update to the database
        uow.Repository<Lesson>().Update(existingLesson);
        await uow.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
