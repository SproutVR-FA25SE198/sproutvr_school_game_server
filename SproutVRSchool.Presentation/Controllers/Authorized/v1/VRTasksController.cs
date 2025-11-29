using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.GetVRTaskById;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRTasks.Queries.SearchVRTasks;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[Authorize]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/vrtasks")]
public sealed class VRTasksController(IMediator mediator) : BaseApiController
{
    // =======================
    // === GETs
    // =======================

    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchVRTasksQueryResponseDto>>> SearchVRTasks(
        [FromQuery] AuthorizedSearchVRTasksQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchVRTasksQuery(@params);

        GetListResultResponseDto<AuthorizedSearchVRTasksQueryResponseDto> result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVRTaskById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new AuthorizedGetVRTaskByIdQuery(id);

        AuthorizedGetVRTaskByIdQueryResponseDto response = await mediator.Send(query, cancellationToken);

        return Ok(response);
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

