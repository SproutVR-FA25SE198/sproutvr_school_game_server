using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.CreateLesson;
using SproutVRSchool.Application.RequestHandlers.Teacher.Lessons.Commands.UpdateLesson;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

[Authorize(Roles = AppCts.Db.ROLE_TEACHER)]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/teacher/lessons")]
public class LessonController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/teacher/lessons

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/teacher/lessons
    // application-type: multipart/form-data
    [HttpPost]
    public async Task<IActionResult> CreateLesson(
        [FromForm] TeacherCreateLessonCommand createLessonCommand)
    {
        Guid lessonId = await mediator.Send(createLessonCommand);

        // 201: Created
        return CreatedAtAction(
            null,
            null,
            new { id = lessonId }
        );
    }

    // ========================
    // === PUTs
    // ========================

    // PUT: api/v1/teacher/lessons/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLesson(
        [FromRoute] Guid id,
        [FromForm] TeacherUpdateLessonCommand updateLessonCommand)
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
