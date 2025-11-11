using MediatR;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.CreateVRLesson;

public sealed class TeacherCreateVRLessonCommandHandler(
    IUnitOfWork uow
    ) : IRequestHandler<TeacherCreateVRLessonCommand, Guid>
{
    public async Task<Guid> Handle(TeacherCreateVRLessonCommand request, CancellationToken cancellationToken)
    {
        // 1. Create VRLesson
        var newVRLesson = VRLesson.Create(
            request.LessonId,
            request.MapId,
            request.Name,
            request.Description,
            request.MaxDuration);

        uow.Repository<VRLesson>().Add(newVRLesson);

        // 2. Create VRTasks
        foreach (CreateVRLessonTaskRequestDto taskDto in request.Tasks)
        {
            var newVRTask = VRTask.Create(
                taskDto.TaskLocationId,
                taskDto.MapObjectId,
                taskDto.ActivityTypeId,
                newVRLesson.Id,
                taskDto.TaskNumber,
                question: taskDto.Question,
                taskDto.Description);
            uow.Repository<VRTask>().Add(newVRTask);
        }

        // 3. Save to DB in a single transaction
        await uow.SaveChangesAsync(cancellationToken);

        return newVRLesson.Id;
    }
}
