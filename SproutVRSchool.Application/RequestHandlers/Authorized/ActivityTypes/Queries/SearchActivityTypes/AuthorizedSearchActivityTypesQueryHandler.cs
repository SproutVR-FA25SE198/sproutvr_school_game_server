using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.ActivityTypes;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.SearchActivityTypes;

public class AuthorizedSearchActivityTypesQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchActivityTypesQuery, GetListResultResponseDto<AuthorizedSearchActivityTypesQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchActivityTypesQueryResponseDto>> Handle(
        AuthorizedSearchActivityTypesQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new ActivityTypesSpecification(request.AuthorizedSearchActivityTypesQueryParams);

        (IReadOnlyList<ActivityType> Data, int Count) rawLists =
            await uow.Repository<ActivityType>().ListAsync(spec);

        var items = rawLists.Data.Select(activityType => new AuthorizedSearchActivityTypesQueryResponseDto(
            Id: activityType.Id,
            ActivityCode: activityType.ActivityCode,
            Name: activityType.Name,
            CreatedAtUtc: activityType.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(activityType.CreatedAtUtc)
        )).ToList();

        return new GetListResultResponseDto<AuthorizedSearchActivityTypesQueryResponseDto>(
            pageSize: request.AuthorizedSearchActivityTypesQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchActivityTypesQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );
    }
}
