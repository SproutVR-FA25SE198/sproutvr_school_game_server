using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Lessons.CreateLesson;
using SproutVRSchool.Application.RequestHandlers.Lessons.GetLessonById;
using SproutVRSchool.Application.RequestHandlers.Lessons.UpdateLesson;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/teacher/lessons")]
public class LessonController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/teacher/lessons
    // GET: api/v1/teacher/lessons/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLessonById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
        )
    {
        var query = new GetLessonByIdQuery(id);
        GetLessonByIdResponseDto lessonResponseDto = await mediator.Send(query, cancellationToken);

        return Ok(lessonResponseDto);
    }

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/teacher/lessons
    // application-type: multipart/form-data
    [HttpPost]
    public async Task<IActionResult> CreateLesson(
        [FromForm] CreateLessonCommand createLessonCommand)
    {
        Guid lessonId = await mediator.Send(createLessonCommand);

        // 201: Created
        return CreatedAtAction(
            nameof(GetLessonById),
            new { id = lessonId },
            new { Id = lessonId });
    }

    // ========================
    // === PUTs
    // ========================

    // PUT: api/v1/teacher/lessons/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson(
        [FromRoute] Guid id,
        [FromForm] UpdateLessonCommand updateLessonCommand)
    {
        updateLessonCommand.LessonId = id;
        await mediator.Send(updateLessonCommand);

        // 204: No Content
        return NoContent();
    }

    // ========================
    // === PATCHs
    // ========================

    // PATCH: api/v1/teacher/lessons/{id}/resource-file

    // ========================
    // === DELETEs
    // ========================

    // DELETE: api/v1/teacher/lessons/{id}
}
