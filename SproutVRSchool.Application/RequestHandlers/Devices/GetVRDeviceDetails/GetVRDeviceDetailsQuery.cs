using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Devices.GetVRDeviceDetails;

public record GetVRDeviceDetailsQuery(
    Guid Id) : IRequest<GetVRDeviceDetailsResponseDto>;
