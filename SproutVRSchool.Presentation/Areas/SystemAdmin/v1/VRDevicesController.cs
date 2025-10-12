
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Devices.SearchVRDevices;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Areas.SystemAdmin.v1;

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
        [FromQuery] SearchVRDevicesParams @params)
    {
        var query = new SearchVRDevicesQuery(@params);
        PaginatedResultDto<SearchVRDevicesResponseDto> result = await mediator.Send(query);
        return Ok(result);
    }

    // GET: api/v1/system-admin/vrdevices?serialNumber=123
    // GET: api/v1/system-admin/vrdevices?status=0
    // GET: api/v1/system-admin/vrdevices/{id}/summaries
    // GET: api/v1/system-admin/vrdevices/{id}/task-progresses    

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

    // PATCH: api/v1/system-admin/vr-devices/{id}/status

    // ========================
    // === DELETEs
    // ========================

    // DELETE: api/v1/system-admin/vr-devices/{id}
}
