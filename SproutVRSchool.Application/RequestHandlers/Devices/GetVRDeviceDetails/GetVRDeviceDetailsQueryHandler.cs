using MediatR;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.Devices.GetVRDeviceDetails;

public sealed class GetVRDeviceDetailsQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetVRDeviceDetailsQuery, GetVRDeviceDetailsResponseDto>
{
    public async Task<GetVRDeviceDetailsResponseDto> Handle(GetVRDeviceDetailsQuery request, CancellationToken cancellationToken)
    {
        var spec = new VRDevicesSpecification(request.Id);

        VRDevice device = await uow.Repository<VRDevice>().GetEntityBySpec(spec);

        if (device == null)
        {
            throw new SvrNotFoundException($"VR Device with ID '{request.Id}' was not found.");
        }

        // 1. Map progresses using the new DTO name
        var progresses = device.VRDeviceTaskProgresses
            .Select(p => new GetVRDeviceDetailsTasksResponseDto(
                p.Id,
                p.VRTaskId,
                p.IsCompleted,
                p.IsCorrect,
                p.CompletionTimeAtUtc
            )).ToList();

        // 2. Map summaries using the new DTO name
        var summaries = device.VRDeviceSessionSummaries
            .Select(s => new GetVRDeviceDetailsSessionSummariesResponseDto(
                s.Id,
                s.StudentName,
                s.NoTasksCompleted
            )).ToList();

        // 3. Return the final DTO using the new name and its nested collections
        return new GetVRDeviceDetailsResponseDto(
            device.Id,
            device.Name,
            new StatusDto(device.Status),
            progresses,
            summaries
        );
    }
}
