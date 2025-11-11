using System.Security.Claims;
using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.ImportAccountsFromExcel;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Presentation.Controllers.Auth.v1;

namespace SproutVRSchool.Presentation.Controllers.Authorized.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/authorized/accounts")]
public sealed class AccountsController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/authorized/accounts
    [HttpGet]
    public async Task<IActionResult> SearchAccounts([FromQuery] AuthorizedSearchAccountsQueryParams queryParams, CancellationToken cancellationToken)
    {
        queryParams.ApplyPagingDefaults();
        var request = new AuthorizedSearchAccountsQuery(queryParams);
        GetListResultResponseDto<AuthorizedSearchAccountsQueryResponseDto> response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    // ========================
    // === POSTs
    // ========================

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}


