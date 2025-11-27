using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;

public class AuthorizedSearchVRLearningSessionsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchVRLearningSessionsQuery, GetListResultResponseDto<AuthorizedSearchVRLearningSessionsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchVRLearningSessionsQueryResponseDto>> Handle(
        AuthorizedSearchVRLearningSessionsQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Specification
        var spec = new VRLearningSessionsSpecification(request.Params);

        // 2. Get the list of vr learning sessions
        (IReadOnlyList<VRLearningSession> Data, int Count) rawLists =
            await uow.Repository<VRLearningSession>().ListAsync(spec);

        // 3. Map the entity to the vr learning sessions
        var items = rawLists.Data.Select(session => new AuthorizedSearchVRLearningSessionsQueryResponseDto(
            Id: session.Id,
            ClassName: session.ClassName,
            StartTimeAtUtc: session.StartTimeAtUtc,
            EndTimeAtUtc: session.EndTimeAtUtc,
            Status: new StatusDto(session.Status),
            DurationInMinutes: session.DurationInMinutes,

            VRLesson: new AuthorizedSearchVRLearningSessionsQueryLessonResponseDto(
                Id: session.VRLessonId,
                Name: session.VRLesson.Name),

            Teacher: new AuthorizedSearchVRLearningSessionsQueryTeacherResponseDto(
                Id: session.TeacherId,
                Name: session.Teacher.GetFullName()),

            CreatedAtUtc: session.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(session.CreatedAtUtc)
        )).ToList();

        // 4. Return the paginated response (no change)
        return new GetListResultResponseDto<AuthorizedSearchVRLearningSessionsQueryResponseDto>(
            pageSize: request.Params.PageSize,
            pageIndex: request.Params.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );
    }
}
