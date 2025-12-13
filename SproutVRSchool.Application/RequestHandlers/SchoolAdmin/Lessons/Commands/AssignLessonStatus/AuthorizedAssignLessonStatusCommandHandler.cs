using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Lessons.Commands.AssignLessonStatus;

public sealed class AuthorizedAssignLessonStatusCommandHandler(
    IUnitOfWork uow,
    ILogger<AuthorizedAssignLessonStatusCommandHandler> logger
    )
    : IRequestHandler<AuthorizedAssignLessonStatusCommand, AuthorizedAssignLessonStatusCommandResponseDto>
{
    public async Task<AuthorizedAssignLessonStatusCommandResponseDto> Handle(
        AuthorizedAssignLessonStatusCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find existing lesson or throw
        Lesson? lesson = await uow.Repository<Lesson>().GetEntityByIdAsync(
            request.LessonId
        ) ?? throw new SvrResourceNotFoundException($"Lesson with ID {request.LessonId} not found.");

        // 2. Assign status
        lesson.UpdateStatus(request.Status);

        // 3. Save changes
        uow.Repository<Lesson>().Update(lesson);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Lesson '{LessonName}' status updated to {Status}", lesson.Name, lesson.Status);

        return new AuthorizedAssignLessonStatusCommandResponseDto(
            LessonId: request.LessonId,
            LessonName: lesson.Name,
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(lesson.Status)
        );
    }
}

