
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.GetVRDevice;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRDevices.Queries.SearchVRDevices;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/vrdevices")]
public sealed class VRDevicesController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // GET: api/v1/authorized/vrdevices
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchVRDevicesResponseDto>>> SearchDevices(
        [FromQuery] AuthorizedSearchVRDevicesParams @params,
        CancellationToken cancellationToken
        )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchVRDevicesQuery(@params);
        GetListResultResponseDto<AuthorizedSearchVRDevicesResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // GET: api/v1/authorized/vrdevices/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDevice(Guid id, CancellationToken cancellationToken)
    {
        var query = new AuthorizedGetVRDeviceQuery(id);
        AuthorizedGetVRDeviceQueryResponseDto result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    // ========================
    // === POSTs
    // ========================

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
