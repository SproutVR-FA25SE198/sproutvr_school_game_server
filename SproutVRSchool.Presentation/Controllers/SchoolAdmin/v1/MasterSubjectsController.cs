using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.MasterSubjects.Commands.AssignMasterSubjectStatus;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;

[Authorize(Roles = AppCts.Db.ROLE_SCHOOL_ADMIN)]
[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/master-subjects")]
public class MasterSubjectsController(IMediator mediator) : BaseApiController
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

    // PATCH: api/v1/school-admin/master-subjects/{id}/assign-status
    [HttpPatch("{id}/assign-status")]
    public async Task<IActionResult> AssignStatus(
      [FromRoute] Guid id,
      [FromBody] SAAssignMasterSubjectStatusCommand request,
      CancellationToken cancellationToken)
    {
        request.MasterSubjectId = id;
        SAAssignMasterSubjectStatusCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
