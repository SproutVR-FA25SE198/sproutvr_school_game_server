using SproutVRSchool.Application.Commons.Responses;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.AssignMapStatus;

public sealed record SAAssignMapStatusCommandResponseDto(
    Guid MapId,
    string MapName,
    string Message,
    StatusDto Status
);
