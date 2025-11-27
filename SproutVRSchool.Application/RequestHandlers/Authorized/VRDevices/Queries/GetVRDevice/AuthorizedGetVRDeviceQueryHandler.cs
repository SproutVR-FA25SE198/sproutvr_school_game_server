using MediatR;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.GetVRDevice;

public sealed class AuthorizedGetVRDeviceQueryHandler(IUnitOfWork uow, IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AuthorizedGetVRDeviceQuery, AuthorizedGetVRDeviceQueryResponseDto>
{
    public async Task<AuthorizedGetVRDeviceQueryResponseDto> Handle(AuthorizedGetVRDeviceQuery request, CancellationToken cancellationToken)
    {
        var spec = new VRDevicesSpecification(request.Id);

        VRDevice device = await uow.Repository<VRDevice>().GetEntityBySpec(spec);

        if (device == null)
        {
            throw new SvrResourceNotFoundException($"VR Device with ID '{request.Id}' was not found.");
        }

        // 1. Map progresses using the new DTO name
        var progresses = device.VRDeviceTaskProgresses
            .Select(p => new AuthorizedGetVRDeviceQueryTasksResponseDto(
                p.Id,
                p.VRTaskId,
                p.IsCompleted,
                p.IsCorrect,
                p.CompletionTimeAtUtc
            )).ToList();

        // 2. Map summaries using the new DTO name
        var summaries = device.VRDeviceSessionSummaries
            .Select(s => new AuthorizedGetVRDeviceQuerySessionSummariesResponseDto(
                s.Id,
                s.StudentName,
                s.NoTasksCompleted
            )).ToList();

        // 3. Return the final DTO using the new name and its nested collections
        return new AuthorizedGetVRDeviceQueryResponseDto(
            device.Id,
            device.Name,
            device.SerialNumber,
            new StatusDto(device.Status),
            progresses,
            summaries,
            device.CreatedAtUtc,
            dateTimeProvider.ConvertToVietNamTime(device.CreatedAtUtc)
        );
    }
}
