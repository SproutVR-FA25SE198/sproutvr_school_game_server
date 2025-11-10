using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.Maps;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.AssignMapStatus;

public sealed class SAAssignMapStatusCommandHandler(
    IUnitOfWork uow,
    ILogger<SAAssignMapStatusCommandHandler> logger
    )
    : IRequestHandler<SAAssignMapStatusCommand, SAAssignMapStatusCommandResponseDto>
{
    public async Task<SAAssignMapStatusCommandResponseDto> Handle(
        SAAssignMapStatusCommand request,
        CancellationToken cancellationToken)
    {
        Map? map = await uow.Repository<Map>().GetEntityByIdAsync(
            request.MapId
        ) ?? throw new SvrResourceNotFoundException($"Map with ID {request.MapId} not found.");

        map.UpdateStatus(request.Status);

        uow.Repository<Map>().Update(map);
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Map '{MapName}' status updated to {Status}", map.Name, map.Status);

        return new SAAssignMapStatusCommandResponseDto(
            MapId: request.MapId,
            MapName: map.Name,
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(map.Status)
        );
    }
}
