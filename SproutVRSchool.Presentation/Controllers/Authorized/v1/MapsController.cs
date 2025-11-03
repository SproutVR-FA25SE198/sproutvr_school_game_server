using System.Security.Cryptography.X509Certificates;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NRedisStack.Search.Aggregation;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Maps.Querries.SearchMaps;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.VRDevices;

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

