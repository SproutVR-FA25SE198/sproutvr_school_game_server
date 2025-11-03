using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Subjects.Queries.SearchSubjects;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/subjects")]
public class SubjectsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/subjects

    [HttpGet]
    public async Task<ActionResult<GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto>>> SearchSubjects(
        [FromQuery] AuthorizedSearchSubjectsQueryParams @params,
        CancellationToken cancellationToken
    )
    {
        @params.ApplyPagingDefaults();
        var query = new AuthorizedSearchSubjectsQuery(@params);
        GetListResultResponseDto<AuthorizedSearchSubjectsQueryResponseDto> result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }



    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}
