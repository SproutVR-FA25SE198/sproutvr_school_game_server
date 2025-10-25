using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Lessons.CreateLesson;
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

    // GET: api/v1/teacher/lessons/{id}


    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/teacher/lessons
    // application-type: multipart/form-data

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
