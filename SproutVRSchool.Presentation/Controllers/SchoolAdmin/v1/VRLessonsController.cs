using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRLessons.Commands.AssignVRLesson;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/vrlessons")]
public class VRLessonsController(IMediator mediator) : BaseApiController
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

    // PATCH: api/v1/school-admin/vrlessons/{id}/assign-status
    [HttpPatch("{id}/assign-status")]
    public async Task<IActionResult> AssignStatus(
       [FromRoute] Guid id,
       [FromBody] SAAssignVRLessonStatusCommand request,
       CancellationToken cancellationToken)
    {
        request.VRLessonId = id;
        SAAssignVRLessonStatusCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
