using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.AssignMapStatus;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.SeedMapBundle;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;


[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/maps")]
public class MapsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/school-admin/maps/seed-bundle
    [HttpPost("seed-bundle")]
    public async Task<IActionResult> SeedMapBundle([FromBody] SASeedMapBundleCommand request)
    {
        SASeedMapBundleCommandResponseDto result = await mediator.Send(request);
        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

    // PATCH: api/v1/school-admin/maps/{id}/assign-status
    [HttpPatch("{id}/assign-status")]
    public async Task<IActionResult> AssignStatus(
    [FromRoute] Guid id,
    [FromBody] SAAssignMapStatusCommand request,
    CancellationToken cancellationToken)
    {
        request.MapId = id;
        SAAssignMapStatusCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
