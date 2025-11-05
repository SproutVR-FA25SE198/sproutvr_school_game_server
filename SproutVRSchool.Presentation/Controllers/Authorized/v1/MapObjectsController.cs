
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.MapObjects.Queries.SearchMapObjects;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/map-objects")]
public class MapObjectsController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // GET: api/v1/authorized/map-objects
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchMapObjectsQueryResponseDto>>> SearchMapObjects(
     [FromQuery] AuthorizedSearchMapObjectsQueryParams @params,
     CancellationToken cancellationToken)
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchMapObjectsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchMapObjectsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
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

