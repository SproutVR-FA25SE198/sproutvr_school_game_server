using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.MasterSubjects.Queries.SearchMasterSubjects;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/master-subjects")]
public class MasterSubjectsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/master-subjects

    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchMasterSubjectsQueryResponseDto>>> SearchMasterSubjects(
        [FromQuery] AuthorizedSearchMasterSubjectsQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchMasterSubjectsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchMasterSubjectsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}
