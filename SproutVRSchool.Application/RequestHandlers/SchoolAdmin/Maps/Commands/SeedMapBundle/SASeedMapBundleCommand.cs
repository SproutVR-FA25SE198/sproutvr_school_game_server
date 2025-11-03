using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.SeedMapBundle;

public sealed record SASeedMapBundleCommand(Guid MapId, string DownloadUrl)
    : IRequest<SASeedMapBundleCommandResponseDto>
{
}
