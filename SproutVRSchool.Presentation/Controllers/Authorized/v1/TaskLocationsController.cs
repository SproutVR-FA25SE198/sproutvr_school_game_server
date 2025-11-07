using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.GetTaskLocationById;
using SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.SearchTaskLocations;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/task-locations")]
public class TaskLocationsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/task-locations
    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchTaskLocationsQueryResponseDto>>> SearchTaskLocations(
        [FromQuery] AuthorizedSearchTaskLocationsQueryParams @params,
        CancellationToken cancellationToken)
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchTaskLocationsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchTaskLocationsQueryResponseDto> result =
            await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskLocationById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new AuthorizedGetTaskLocationByIdQuery(id);

        AuthorizedGetTaskLocationByIdQueryResponseDto response = await mediator.Send(query, cancellationToken);

        return Ok(response);
    }


    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}
