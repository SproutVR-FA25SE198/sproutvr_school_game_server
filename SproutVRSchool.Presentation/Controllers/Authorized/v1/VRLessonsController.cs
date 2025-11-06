using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.TaskLocations.Queries.SearchTaskLocations;
using SproutVRSchool.Application.RequestHandlers.Authorized.VRLessons.Queries.GetVRLessonById;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/vrlessons")]
public class VRLessonsController(IMediator mediator) : BaseApiController
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
        var query = new TeacherGetVRLessonByIdQuery(id);
        TeacherGetVRLessonByIdResponseDto result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

}
