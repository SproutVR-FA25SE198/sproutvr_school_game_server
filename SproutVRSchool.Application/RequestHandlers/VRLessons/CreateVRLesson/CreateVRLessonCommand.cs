using MediatR;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Application.RequestHandlers.VRLessons.CreateVRLesson;

public record CreateVRLessonCommand(
    Guid LessonId,
    Guid MapId,
    string Name,
    string Description,
    TimeSpan MaxDuration,
    List<CreateVRLessonTaskRequestDto> Tasks) : IRequest<Guid>;
