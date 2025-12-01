using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.RequestHandlers.Auth.Commands.ChangePassword;
using SproutVRSchool.Application.RequestHandlers.Auth.Commands.Login;
using SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;
using SproutVRSchool.Application.RequestHandlers.Authorized.Accounts.Queries.SearchAccounts;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Auth.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // GET: api/v1/auth/profile
    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetCurrentUserProfile(CancellationToken cancellationToken)
    {
        var request = new AuthGetCurrentUserCommand();
        AuthGetCurrentUserCommandResponseDto response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginCommand request, CancellationToken cancellationToken)
    {
        AuthLoginCommandResponseDto response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================

    // PATCH: api/v1/auth/change-password
    [Authorize]
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] AuthChangePasswordCommand request, CancellationToken cancellationToken)
    {
        AuthChangePasswordCommandResponseDto response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}
