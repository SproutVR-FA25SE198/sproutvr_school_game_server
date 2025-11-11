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
                Id: taskLocation.Map.Id,
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
}
