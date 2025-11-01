using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Maps.SeedMapBundle;
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

    [HttpPost("seed-bundle")]
    public async Task<IActionResult> SeedMapBundle([FromBody] SeedMapBundleCommand request)
    {
        SeedMapBundleResponseDto result = await mediator.Send(request);
        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}
