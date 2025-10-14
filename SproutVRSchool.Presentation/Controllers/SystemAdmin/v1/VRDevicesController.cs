
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Devices.GetVRDeviceDetails;
using SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SystemAdmin.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/system-admin/vrdevices")]
public sealed class VRDevicesController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // GET: api/v1/system-admin/vrdevices
    //          ?searchName=abc&status=0
    //          &pageIndex=1&pageSize=10
    //          &sortBy=status
    //          

    [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<SearchVRDevicesResponseDto>>> SearchDevices(
        [FromQuery] SearchVRDevicesParams @params,
        CancellationToken cancellationToken
        )
    {
        var query = new SearchVRDevicesQuery(@params);
        PaginatedResultDto<SearchVRDevicesResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // GET: api/v1/system-admin/vrdevices/{id}details
    [HttpGet("{id}/summaries")]
    public async Task<IActionResult> GetDeviceDetails(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetVRDeviceDetailsQuery(id);
        object result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/system-admin/vr-devices

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

    // ========================
    // === DELETEs
    // ========================

    // DELETE: api/v1/system-admin/vr-devices/{id}
}
