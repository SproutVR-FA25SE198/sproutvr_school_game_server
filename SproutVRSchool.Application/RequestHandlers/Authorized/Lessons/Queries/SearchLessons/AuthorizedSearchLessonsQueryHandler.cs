using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.SearchLessons;

public class AuthorizedSearchLessonsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchLessonsQuery, GetListResultResponseDto<AuthorizedSearchLessonsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchLessonsQueryResponseDto>> Handle(
        AuthorizedSearchLessonsQuery request,
        CancellationToken cancellationToken)
    {
        (IReadOnlyList<Lesson> Data, int Count) rawLists = await uow.Repository<Lesson>()
            .ListAsync(new LessonsSpecification(request.AuthorizedSearchLessonsQueryParams));

        var items = rawLists.Data.Select(lesson => new AuthorizedSearchLessonsQueryResponseDto(
            Id: lesson.Id,
            Name: lesson.Name,
            Description: lesson.Description,
            ResourceRelativeFilePath: lesson.ResourceRelativeFilePath,
            VRLessonsCount: lesson.VRLessons.Count,
            Status: new StatusDto(lesson.Status),
            Subject: new AuthorizedSearchLessonsQuerySubjectDto(
                Id: lesson.Subject.Id,
                Name: lesson.Subject.Name,
                Description: lesson.Subject.Description,
                ImageUrl: lesson.Subject.ImageUrl
            ),
            MasterSubject: new AuthorizedSearchLessonsQueryMasterSubjectDto(
                Id: lesson.Subject.MasterSubject.Id,
                Name: lesson.Subject.MasterSubject.Name,
                Description: lesson.Subject.MasterSubject.Description,
                ImageUrl: lesson.Subject.MasterSubject.ImageUrl
            ),
            Teacher: new AuthorizedSearchLessonsQueryTeacherDto(
                Id: lesson.Teacher.Id,
                FirstName: lesson.Teacher.FirstName,
                LastName: lesson.Teacher.LastName
            ),
            CreatedAtUtc: lesson.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(lesson.CreatedAtUtc)
        )).ToList();

        return new GetListResultResponseDto<AuthorizedSearchLessonsQueryResponseDto>(
            pageSize: request.AuthorizedSearchLessonsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchLessonsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );
    }
}
