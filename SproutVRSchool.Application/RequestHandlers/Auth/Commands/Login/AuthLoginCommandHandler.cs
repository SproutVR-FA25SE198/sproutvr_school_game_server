using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SproutVRSchool.Application.Abstractions.AccountServices;
using SproutVRSchool.Application.Exceptions.Accounts;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.Login;

public sealed class AuthLoginCommandHandler
        (UserManager<UserAccount> userManager,
        ITokenService tokenService,
        IConfiguration configuration) : IRequestHandler<AuthLoginCommand, AuthLoginCommandResponseDto>
{
    public async Task<AuthLoginCommandResponseDto> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
    {
        // 1. The identifier could be username or email
        UserAccount user = await userManager.FindByEmailAsync(request.Identifier)
            ?? await userManager.FindByNameAsync(request.Identifier)
            ?? throw new SvrUnauthorizedAccessException("Invalid username or email.");

        // 2. Check if user is active
        if (user.Status != UserAccountStatus.Active)
        {
            throw new SvrUnauthorizedAccessException("User account is not active.");
        }

        // 3. Check password
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new SvrUnauthorizedAccessException("Invalid password.");
        }

        // 4. Get roles + lifetime
        IList<string> roles = await userManager.GetRolesAsync(user);
        int tokenLifetimeMinutes = configuration.GetValue<int>("Jwt:AccessTokenExpirationMinutes");

        // 5. Generate token
        (string token, DateTimeOffset expiredAt) = tokenService.GenerateToken(
            user,
            roles,
            TimeSpan.FromMinutes(tokenLifetimeMinutes));

        return new AuthLoginCommandResponseDto(token, expiredAt);
    }
}
