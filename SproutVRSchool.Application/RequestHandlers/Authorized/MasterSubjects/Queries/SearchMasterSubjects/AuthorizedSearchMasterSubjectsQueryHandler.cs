using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.SearchMasterSubjects;

public class AuthorizedSearchMasterSubjectsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchMasterSubjectsQuery, GetListResultResponseDto<AuthorizedSearchMasterSubjectsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchMasterSubjectsQueryResponseDto>> Handle(
        AuthorizedSearchMasterSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        // Get items & count from params
        (IReadOnlyList<MasterSubject> Data, int Count) rawLists = await uow.Repository<MasterSubject>()
            .ListAsync(new MasterSubjectsSpecification(request.AuthorizedSearchMasterSubjectsQueryParams));

        // Build DTO list
        var items = rawLists.Data.Select(masterSubject => new AuthorizedSearchMasterSubjectsQueryResponseDto(
            Id: masterSubject.Id,
            Name: masterSubject.Name,
            Description: masterSubject.Description,
            ImageUrl: masterSubject.ImageUrl,
            Status: new StatusDto(masterSubject.Status),
            CreatedAtUtc: masterSubject.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(masterSubject.CreatedAtUtc)
        )).ToList();

        // Wrap into paged result
        var result = new GetListResultResponseDto<AuthorizedSearchMasterSubjectsQueryResponseDto>(
            pageSize: request.AuthorizedSearchMasterSubjectsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchMasterSubjectsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );

        return result;
    }
}
