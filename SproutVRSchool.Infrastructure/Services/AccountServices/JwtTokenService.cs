using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SproutVRSchool.Application.Abstractions.AccountServices;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Services.AccountServices;

public sealed class JwtTokenService(
    IDateTimeProvider dateTimeProvider,
    IConfiguration configuration) : ITokenService
{
    public (string Token, DateTimeOffset ExpiredAtVietNam) GenerateToken(UserAccount user, IList<string> roles, TimeSpan durationInMinutes)
    {
        // 1. Get settings 
        string secretKey = configuration.GetValue<string>("Jwt:SecretKey")
            ?? throw new InvalidOperationException("JWT SecretKey not configured");

        string issuer = configuration.GetValue<string>("Jwt:Issuer")
            ?? throw new InvalidOperationException("JWT Issuer not configured");

        string[] audiences = configuration
            .GetSection("Jwt:Audiences")
            .Get<string[]>()
            ?? throw new InvalidOperationException("JWT Audiences not configured");

        // 2. Create signing credentials
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 4. Add Claims
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new("status", user.Status.ToString()),
            new("roles", JsonConvert.SerializeObject(roles), JsonClaimValueTypes.JsonArray),
        };

        // 5. Create the token
        DateTimeOffset expiresAtUtc = dateTimeProvider.UtcDateTimeNow.Add(durationInMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Subject = new ClaimsIdentity(claims),
            Claims = claims.ToDictionary(c => c.Type, c => (object)c.Value),
            Audience = string.Join(",", audiences),
            NotBefore = dateTimeProvider.UtcDateTimeNow.UtcDateTime,
            Expires = expiresAtUtc.UtcDateTime,
            SigningCredentials = creds
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(descriptor);

        return (token, expiresAtUtc);
    }

    public async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        // 1. Get settings 
        string secretKey = configuration.GetValue<string>("Jwt:SecretKey")
            ?? throw new InvalidOperationException("JWT SecretKey not configured");

        string issuer = configuration.GetValue<string>("Jwt:Issuer")
            ?? throw new InvalidOperationException("JWT Issuer not configured");

        string[] audiences = configuration
            .GetSection("Jwt:Audiences")
            .Get<string[]>()
            ?? throw new InvalidOperationException("JWT Audiences not configured");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var tokenHandler = new JsonWebTokenHandler();

        // 2. Validate claims
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,

            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudiences = audiences,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // no tolerance for expired tokens
        };

        // 3. Validate the token
        TokenValidationResult result = await tokenHandler.ValidateTokenAsync(token, validationParameters);

        if (!result.IsValid)
        {
            return null;
        }

        return result.ClaimsIdentity != null
            ? new ClaimsPrincipal(result.ClaimsIdentity)
            : null;
    }
}
