using System.Security.Claims;
using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.GetTeacherAccountById;
using SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;
using SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.ImportAccountsFromExcel;
using SproutVRSchool.Domain;
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

    // GET: api/v1/authorized/accounts/:id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new AuthorizedGetAccountByIdQuery { Id = id };
        AuthorizedGetAccountByIdQueryResponseDto response =
            await mediator.Send(query, cancellationToken);
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
