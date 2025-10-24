using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Lessons.CreateLesson;
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
    public async Task<IActionResult> GetLessonById(Guid id)
    {

        return Ok(new { Id = id });
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
    public async Task<IActionResult> UpdateLesson(Guid id,
        [FromForm] UpdateLessonCommand updateLessonCommand)
    {

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
