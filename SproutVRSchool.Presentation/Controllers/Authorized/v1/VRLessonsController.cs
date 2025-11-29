using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.SearchVRLessons;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[Authorize]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/vrlessons")]
public sealed class VRLessonsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/vrlessons/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVRLesson(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new AuthorizedGetVRLessonByIdQuery(id);
        AuthorizedGetVRLessonByIdQueryResponseDto result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // GET: api/v1/authorized/vrlessons
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchVRLessonsQueryResponseDto>>> SearchVRLessons(
        [FromQuery] AuthorizedSearchVRLessonsQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchVRLessonsQuery(@params);

        GetListResultResponseDto<AuthorizedSearchVRLessonsQueryResponseDto> result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

}


