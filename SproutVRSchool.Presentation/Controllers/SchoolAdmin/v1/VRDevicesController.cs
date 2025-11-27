
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.AssignVRDeviceStatus;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.ImportVRDevicesFromExcel;
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

    // POST: api/v1/school-admin/vrdevices/import
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

    // PATCH: api/v1/school-admin/vrdevices/assign-status
    [HttpPatch("{id}/assign-status")]
    public async Task<IActionResult> AssignStatus(
        [FromRoute] Guid id,
        [FromBody] SAAssignVRDeviceStatusCommand request,
        CancellationToken cancellationToken)
    {
        request.DeviceId = id;
        SAAssignVRDeviceStatusCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    // ========================
    // === DELETEs
    // ========================
}
