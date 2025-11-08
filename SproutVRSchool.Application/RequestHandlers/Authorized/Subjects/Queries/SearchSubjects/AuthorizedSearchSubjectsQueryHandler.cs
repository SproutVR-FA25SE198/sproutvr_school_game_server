using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;

public class AuthorizedSearchSubjectsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchSubjectsQuery, GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>> Handle(
        AuthorizedSearchSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        // Get items & count from params
        (IReadOnlyList<Subject> Data, int Count) rawLists = await uow.Repository<Subject>()
            .ListAsync(new SubjectsSpecification(request.AuthorizedSearchSubjectsQueryParams));

        var items = rawLists.Data.Select(subject => new AuthorizedSearchSubjectsQueryResponseDto(
            Id: subject.Id,
            MasterSubject: new AuthorizedSearchSubjectsQueryMasterSubjectResponseDto(
                Id: subject.MasterSubject.Id,
                Name: subject.MasterSubject.Name,
                Description: subject.MasterSubject.Description,
                ImageUrl: subject.MasterSubject.ImageUrl,
                Status: new StatusDto(subject.MasterSubject.Status),
                CreatedAtUtc: subject.MasterSubject.CreatedAtUtc,
                CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(subject.MasterSubject.CreatedAtUtc)
            ),
            Name: subject.Name,
            Description: subject.Description,
            ImageUrl: subject.ImageUrl,
            Status: new StatusDto(subject.Status),
            CreatedAtUtc: subject.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(subject.CreatedAtUtc)
        )).ToList();

        var result = new GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>(
            pageSize: request.AuthorizedSearchSubjectsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchSubjectsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );

        return result;
    }
}
