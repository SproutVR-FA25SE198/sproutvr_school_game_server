using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.SearchVRDeviceSessionSummaries;

public class AuthorizedSearchVRDeviceSessionSummariesQueryHandler(
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider
) : IRequestHandler<AuthorizedSearchVRDeviceSessionSummariesQuery, GetListResultResponseDto<AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto>>
{
    public async Task<GetListResultResponseDto<AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto>> Handle(
        AuthorizedSearchVRDeviceSessionSummariesQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Create the specification
        var spec = new VRDeviceSessionSummariesSpecification(request.Params);

        // 2. Get data and count
        (IReadOnlyList<VRDeviceSessionSummary> Data, int Count) rawLists =
            await uow.Repository<VRDeviceSessionSummary>().ListAsync(spec);

        // 3. Map to DTOs
        var items = rawLists.Data.Select(summary => new AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto(
            StudentName: summary.StudentName,
            NoTasksCompleted: summary.NoTasksCompleted,

            // Nested VRDevice DTO
            VRDevice: new AuthorizedSearchVRDeviceSessionSummariesQueryDeviceResponseDto(
                    summary.VRDevice.Id,
                    summary.VRDevice.Name,
                    summary.VRDevice.SerialNumber
                ),

            // Nested VRLearningSession DTO
            VRLearningSession: new AuthorizedSearchVRDeviceSessionSummariesQueryVRLearningSessionResponseDto(
                    summary.VRLearningSession.Id,
                    summary.VRLearningSession.ClassName
                ),

            CreatedAtUtc: summary.CreatedAtUtc,
            CreatedAtVietNam: dateTimeProvider.ConvertToVietNamTime(summary.CreatedAtUtc)
        )).ToList();

        // 4. Return paginated response
        return new GetListResultResponseDto<AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto>(
            pageSize: request.Params.PageSize,
            pageIndex: request.Params.PageIndex,
            totalItems: rawLists.Count,
            items: items
        );
    }
}
