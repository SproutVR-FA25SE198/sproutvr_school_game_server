using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.GetVRLearningSession;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLearningSessions.Queries.SearchVRLearningSessions;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[Authorize]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/vr-learning-sessions")]
public sealed class VRLearningSessionsController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    // GET: api/v1/authorized/vr-learning-sessions
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchVRLearningSessionsQueryResponseDto>>> SearchVRLearningSessions(
      [FromQuery] AuthorizedSearchVRLearningSessionsQueryParams @params,
      CancellationToken cancellationToken)
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchVRLearningSessionsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchVRLearningSessionsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // GET: api/v1/authorized/vr-learning-sessions/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVRLearningSession(Guid id, CancellationToken cancellationToken)
    {
        var query = new AuthorizedGetVRLearningSessionQuery(id);
        AuthorizedGetVRLearningSessionQueryResponseDto result = await mediator.Send(query, cancellationToken);

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
