using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.GetSubjectById;
using SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Subjects.Commands.AssignSubjectStatus;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.SchoolAdmin.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/school-admin/subjects")]
public class SubjectsController(IMediator mediator) : BaseApiController
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

    // PATCH: api/v1/school-admin/subjects/{id}/assign-status
    [HttpPatch("{id}/assign-status")]
    public async Task<IActionResult> AssignStatus(
        [FromRoute] Guid id,
        [FromBody] SAAssignSubjectStatusCommand request,
        CancellationToken cancellationToken)
    {
        request.SubjectId = id;
        SAAssignSubjectStatusCommandResponseDto result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
