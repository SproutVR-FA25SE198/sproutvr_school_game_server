using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.GetVRLearningSession;

public sealed class AuthorizedGetVRLearningSessionQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedGetVRLearningSessionQuery, AuthorizedGetVRLearningSessionQueryResponseDto>
{
    public async Task<AuthorizedGetVRLearningSessionQueryResponseDto> Handle(
        AuthorizedGetVRLearningSessionQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new VRLearningSessionsSpecification(request.Id);

        VRLearningSession session = await uow.Repository<VRLearningSession>().GetEntityBySpec(spec);

        if (session == null)
        {
            throw new SvrResourceNotFoundException($"VR Learning Session with ID '{request.Id}' was not found.");
        }

        // 1. Map progresses
        var progresses = session.VRDeviceTaskProgresses
            .Select(p => new AuthorizedGetVRLearningSessionTaskProgressResponseDto(
                p.Id,
                p.VRDeviceId,
                p.VRTaskId,
                p.StudentName,
                p.IsCompleted,
                p.IsCorrect,
                p.CompletionTimeAtUtc
            )).ToList();

        // 2. Map summaries
        var summaries = session.VRDeviceSessionSummaries
            .Select(s => new AuthorizedGetVRLearningSessionSummaryResponseDto(
                s.Id,
                s.StudentName,
                s.NoTasksCompleted
            )).ToList();

        // 3. Map Lesson 
        var lessonDto = new AuthorizedGetVRLearningSessionLessonResponseDto(
                session.VRLesson.Id,
                session.VRLesson.Name
            );

        // 4. Map Teacher 
        var teacherDto = new AuthorizedGetVRLearningSessionTeacherResponseDto(
                session.Teacher.Id,
                session.Teacher.GetFullName()
            );

        // 5. Return the final DTO
        return new AuthorizedGetVRLearningSessionQueryResponseDto(
            session.Id,
            session.ClassName,
            session.StartTimeAtUtc,
            session.EndTimeAtUtc,
            session.DurationInMinutes,
            new StatusDto(session.Status),
            lessonDto,
            teacherDto,
            progresses,
            summaries,
            session.CreatedAtUtc,
            dateTimeProvider.ConvertToVietNamTime(session.CreatedAtUtc)
        );
    }
}
