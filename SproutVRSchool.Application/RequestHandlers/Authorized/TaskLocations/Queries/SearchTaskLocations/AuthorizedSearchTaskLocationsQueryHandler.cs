using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.SearchTaskLocations;

public class AuthorizedSearchTaskLocationsQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchTaskLocationsQuery, GetListResultResponseDto<AuthorizedSearchTaskLocationsQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchTaskLocationsQueryResponseDto>> Handle(
        AuthorizedSearchTaskLocationsQuery request,
        CancellationToken cancellationToken)
    {
        // Get items & count from repository
        (IReadOnlyList<TaskLocation> Data, int Count) rawLists = await uow.Repository<TaskLocation>()
            .ListAsync(new TaskLocationsSpecification(request.AuthorizedSearchTaskLocationsQueryParams));

        // Build DTO list
        var items = rawLists.Data.Select(taskLocation => new AuthorizedSearchTaskLocationsQueryResponseDto(
            Id: taskLocation.Id,
            LocationCode: taskLocation.LocationCode,
            Name: taskLocation.Name,
            ImageUrl: taskLocation.ImageUrl,
            Map: new AuthorizedSearchTaskLocationsMapQueryResponseDto(
                MapCode: taskLocation.Map.MapCode,
                Name: taskLocation.Map.Name,
                ImageUrl: taskLocation.Map.ImageUrl,
                PreviewUrl: taskLocation.Map.PreviewUrl
            ),
            CreatedAtUtc: taskLocation.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(taskLocation.CreatedAtUtc)
        )).ToList();

        // Wrap into paged result
        var result = new GetListResultResponseDto<AuthorizedSearchTaskLocationsQueryResponseDto>(
            pageSize: request.AuthorizedSearchTaskLocationsQueryParams.PageSize,
            pageIndex: request.AuthorizedSearchTaskLocationsQueryParams.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );

        return result;
    }

    public sealed class TaskLocationsSpecification : BaseSpecification<TaskLocation>
    {
        public TaskLocationsSpecification(AuthorizedSearchTaskLocationsQueryParams searchParams)
            : base(x =>
                (!searchParams.MapId.HasValue || x.MapId == searchParams.MapId.Value) &&
                (string.IsNullOrEmpty(searchParams.LocationCode) || x.LocationCode.Contains(searchParams.LocationCode)) &&
                (string.IsNullOrEmpty(searchParams.Name) || x.Name.Contains(searchParams.Name)))
        {
            // Pagination
            if (searchParams.IsPaginated!.Value && searchParams.PageSize.HasValue && searchParams.PageIndex.HasValue)
            {
                ApplyPaging(searchParams.PageSize.Value * (searchParams.PageIndex.Value - 1), searchParams.PageSize.Value);
            }

            // Sorting
            if (string.IsNullOrEmpty(searchParams.SortBy))
            {
                searchParams.SortBy = AppCts.SortingKeys.DEFAULT;
            }

            switch (searchParams.SortBy)
            {
                case AppCts.SortingKeys.TaskLocations.NAME_ASC:
                    AddOrderBy(x => x.Name);
                    break;
                case AppCts.SortingKeys.TaskLocations.NAME_DESC:
                    AddOrderByDescending(x => x.Name);
                    break;
                case AppCts.SortingKeys.CREATED_AT_UTC_ASC:
                    AddOrderBy(x => x.CreatedAtUtc);
                    break;
                case AppCts.SortingKeys.CREATED_AT_UTC_DESC:
                    AddOrderByDescending(x => x.CreatedAtUtc);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }

            // Include related Map
            AddInclude(x => x.Map);
        }
    }
}
