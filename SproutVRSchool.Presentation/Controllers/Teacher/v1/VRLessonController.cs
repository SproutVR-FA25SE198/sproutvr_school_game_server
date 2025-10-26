using System.Numerics;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.VRLessons.CreateVRLesson;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/teacher/vrlessons")]
public class VRLessonController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    [HttpGet("{id:guid}")]
    public IActionResult GetVRLesson(Guid id)
    {
        return Ok(new { Id = id });
    }

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/teacher/vrlessons
    [HttpPost]
    public async Task<IActionResult> CreateVRLesson(
         [FromBody] CreateVRLessonCommand command,
         CancellationToken cancellationToken)
    {
        Guid vrLessonId = await mediator.Send(command, cancellationToken);

        // 201 Created response
        return CreatedAtAction(
            nameof(GetVRLesson),
            new { id = vrLessonId },
            new { id = vrLessonId });
    }
}
