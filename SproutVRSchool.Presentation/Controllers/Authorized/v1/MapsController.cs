using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Queries.SearchMaps;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;


[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/maps")]
public class MapsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/maps

    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchMapsQueryResponseDto>>> SearchMaps(
        [FromQuery] AuthorizedSearchMapsQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchMapsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchMapsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}

