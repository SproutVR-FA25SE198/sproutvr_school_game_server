using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Lessons.Commands.AssignLessonStatus;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;

[Authorize(Roles = AppCts.Db.ROLE_SCHOOL_ADMIN)]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/lessons")]
public class LessonsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

    // PATCH: api/v1/school-admin/lessons/{id}/assign-status
    [HttpPatch("{id}/assign-status")]
    public async Task<IActionResult> AssignStatus(
    [FromRoute] Guid id,
    [FromBody] SAAssignLessonStatusCommand request,
    CancellationToken cancellationToken)
    {
        request.LessonId = id;
        SAAssignLessonStatusCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}

