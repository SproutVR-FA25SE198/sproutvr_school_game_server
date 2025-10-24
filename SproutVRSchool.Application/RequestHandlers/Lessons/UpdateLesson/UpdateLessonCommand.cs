using MediatR;
using Microsoft.AspNetCore.Http;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

public record UpdateLessonCommand : IRequest<Unit>
{
    public Guid LessonId { get; init; }
    public Guid? SubjectId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IFormFile? ResourceFile { get; init; }
    public LessonStatus? Status { get; init; }
}

public class UpdateLessonCommandHandler(
    IUnitOfWork uow
    ) : IRequestHandler<UpdateLessonCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the existing entity from the database
        Lesson? existingLesson = await uow.Repository<Lesson>().GetEntityByIdAsync(request.LessonId);

        // 2. Check if it exists
        if (existingLesson is null)
        {
            throw new NotFoundException($"Lesson with ID {request.LessonId} not found.");
        }

        // 3. Update the entity's properties with the new values
        existingLesson.Update(
            newName: request.Name,
            newDescription: request.Description,
            newSubjectId: request.SubjectId,
            newStatus: request.Status
        );

        // 4. Tell EF Core to track the update
        lessonRepo.Update(existingLesson);

        // 5. Save all changes
        await uow.SaveChangesAsync(cancellationToken);

        // 6. Return 'Unit.Value' (which is equivalent to 'void')
        return Unit.Value;
    }
}
