using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Application.RequestHandlers.Auth.Commands.Login;
using SproutVRSchool.Application.RequestHandlers.Auth.Commands.SeedAccountsFromExcel;
using SproutVRSchool.Domain;

namespace SproutVRSchool.Presentation.Controllers.Auth.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController(IMediator mediator) : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // ========================
    // === POSTs
    // ========================

    // POST: api/v1/auth/login
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] AuthLoginCommand request)
    {
        AuthLoginCommandResponseDto response = await mediator.Send(request);
        return Ok(response);
    }

    // POST: api/v1/auth/seed-from-excel
    [HttpPost("seed-from-excel")]
    public async Task<IActionResult> SeedFromExcel([FromForm] SeedAccountsFromExcelCommand request)
    {
        SeedAccountsFromExcelResponseDto response = await mediator.Send(request);
        return Ok(response);
    }


    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}


