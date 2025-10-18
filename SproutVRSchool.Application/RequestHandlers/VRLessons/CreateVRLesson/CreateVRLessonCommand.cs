using MediatR;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.CreateVRLesson;

public record CreateVRLessonCommand(
    Guid LessonId,
    Guid MapId,
    string Name,
    string Description,
    TimeSpan MaxDuration,
    string ImageUrl) : IRequest<Guid>;

public class CreateVRLessonCommandHandler(
    IUnitOfWork uow
    ) : IRequestHandler<CreateVRLessonCommand, Guid>
{
    public async Task<Guid> Handle(CreateVRLessonCommand request, CancellationToken cancellationToken)
    {
        // Store the image into the firebase

        var newVRLesson = VRLesson.Create(
            request.LessonId,
            request.MapId,
            request.Name,
            request.Description,
            request.MaxDuration,
            request.ImageUrl);

        uow.Repository<VRLesson>().Add(newVRLesson);
        await uow.Repository<VRLesson>().SaveAllAsync();

        return newVRLesson.Id;
    }
}
