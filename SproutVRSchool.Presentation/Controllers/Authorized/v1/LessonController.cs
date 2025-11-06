using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.GetLessonById;
using SproutVRSchool.Application.RequestHandlers.Authorized.Lessons.Queries.SearchLessons;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/lessons")]
public class LessonsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/lessons

    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchLessonsQueryResponseDto>>> SearchLessons(
        [FromQuery] AuthorizedSearchLessonsQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchLessonsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchLessonsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // GET: api/v1/authorized/lessons/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLesson(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var query = new AuthorizedGetLessonByIdQuery(id);
        AuthorizedGetLessonByIdQueryResponseDto lessonResponseDto = await mediator.Send(query, cancellationToken);

        return Ok(lessonResponseDto);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

}
