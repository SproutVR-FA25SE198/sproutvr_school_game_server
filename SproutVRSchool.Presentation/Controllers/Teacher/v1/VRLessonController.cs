using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Teacher.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/teacher/vrlessons")]
public class VRLessonController(IMediator mediator) : BaseApiController
{

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/teacher/vrlessons
    public async Task<IActionResult> CreateVRLesson(
        [FromBody] Create
        CancellationToken cancellationToken)
    {
        // Method intentionally left empty.
    }
}
