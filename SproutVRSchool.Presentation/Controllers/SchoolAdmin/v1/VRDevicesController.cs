
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Maps.Commands.ImportVRDevicesFromExcel;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/vrdevices")]
public sealed class VRDevicesController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // ========================
    // === POSTs
    // ========================

    [HttpPost("import")]
    public async Task<IActionResult> ImportVRDevicesFromExcel(
        [FromForm] SAImportVRDevicesFromExcelCommand request,
    CancellationToken cancellationToken)
    {
        SAImportVRDevicesFromExcelCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

    // ========================
    // === DELETEs
    // ========================
}


