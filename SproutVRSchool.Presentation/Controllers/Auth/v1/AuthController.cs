using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Presentation.Controllers.Auth.v1;

[ApiVersion(AppCts.Api.V1)]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : BaseApiController
{
    // ========================
    // === GETs
    // ========================

    // ========================
    // === POSTs
    // ========================

    public Task<IActionResult> Login([FromBody] AuthLoginCommand request)
    {

    }


    // ========================
    // === PUTs
    // ========================

    // ========================
    // === PATCHs
    // ========================
}

public sealed record AuthLoginCommand(string Identifier, string Password) : IRequest<AuthLoginCommandReponseDto>
{
}

public class AuthLoginCommandReponseDto
{
    public string AccessToken { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
}

public sealed class AuthLoginCommandHandler
        (UserManager<UserAccount> userManager,
        SignInManager<UserAccount> signInManager,
        IConfiguration configuration) : IRequestHandler<AuthLoginCommand, AuthLoginCommandReponseDto>
{
    public async Task<AuthLoginCommandReponseDto> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
    {
        // The identifier could be username or email
        var user = await userManager.FindByEmailAsync(request.Identifier)
            ?? await userManager.FindByNameAsync(request.Identifier);

        if (user is null)
            throw new UnauthorizedAccessException
    }
}
