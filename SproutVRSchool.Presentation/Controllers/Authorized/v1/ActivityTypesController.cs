
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.ActivityTypes.Queries.SearchActivityTypes;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/activity-types")]
public sealed class ActivityTypesController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // GET: api/v1/authorized/activty-types
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchActivityTypesQueryResponseDto>>> SearchActivityTypes(
      [FromQuery] AuthorizedSearchActivityTypesQueryParams @params,
      CancellationToken cancellationToken)
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchActivityTypesQuery(@params);
        GetListResultResponseDto<AuthorizedSearchActivityTypesQueryResponseDto> result = await mediator.Send(query, cancellationToken);
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
