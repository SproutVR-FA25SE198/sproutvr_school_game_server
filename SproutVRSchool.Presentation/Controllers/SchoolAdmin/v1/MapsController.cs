using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Requests;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.SeedMapBundle;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.VRDevices;

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
}
