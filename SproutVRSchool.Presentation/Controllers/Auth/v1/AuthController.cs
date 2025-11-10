using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.RequestHandlers.Auth.Commands.Login;
using SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Infrastructure.Data;

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
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
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
}

