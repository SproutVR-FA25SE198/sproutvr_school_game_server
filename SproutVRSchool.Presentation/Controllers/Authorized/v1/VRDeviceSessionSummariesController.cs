using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRDeviceSessionSummaries.Queries.SearchVRDeviceSessionSummaries;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

// Assuming a new controller, but you can add this to an existing one
[Route("api/v{version:apiVersion}/authorized/vr-device-session-summaries")]
public class VRDeviceSessionSummariesController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // GET: api/v1/authorized/vr-device-session-summaries
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto>>> SearchVRDeviceSessionSummaries(
      [FromQuery] AuthorizedSearchVRDeviceSessionSummariesQueryParams @params,
      CancellationToken cancellationToken)
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchVRDeviceSessionSummariesQuery(@params);
        GetListResultResponseDto<AuthorizedSearchVRDeviceSessionSummariesQueryResponseDto> result = await mediator.Send(query, cancellationToken);
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


