using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.CreateVRLesson;
using SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Commands.DesignVRLessonPreset;
using SproutVRSchool.Application.RequestHandlers.Teacher.VRLessons.Queries.GetVRLessonById;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/teacher/vrlessons")]
public class VRLessonController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/teacher/vrlessons/{id}
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
    // === POSTs
    // ========================

    // POST: api/v1/teacher/vrlessons
    [HttpPost]
    public async Task<IActionResult> CreateVRLesson(
         [FromBody] TeacherCreateVRLessonCommand command,
         CancellationToken cancellationToken)
    {
        Guid vrLessonId = await mediator.Send(command, cancellationToken);

        // 201 Created response
        return CreatedAtAction(
            nameof(GetVRLesson),
            new { id = vrLessonId },
            new { id = vrLessonId });
    }

    // ========================
    // === PATCHs
    // ========================

    // PATCH: api/v1/teacher/vrlessons/{id}/design-preset
    [HttpPatch("{id:guid}/design-preset")]
    public async Task<IActionResult> DesignVRLessonPreset(
        [FromRoute] Guid id,
        [FromBody] TeacherDesignVRLessonPresetCommand command,
        CancellationToken cancellationToken)
    {
        command.VRLessonId = id;
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
