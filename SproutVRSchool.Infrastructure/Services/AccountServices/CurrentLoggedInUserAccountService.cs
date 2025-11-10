using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using SproutVRSchool.Application.Abstractions.AccountServices;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Services.AccountServices;

public sealed class CurrentLoggedInUserAccountService(
    ClaimsPrincipal _user
    ) : ICurrentLoggedInUserAccountService
{
    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            string? id = _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(id, out Guid guid) ? guid : null;
        }
    }

    public string? Email => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    public string? Status => _user.Claims.FirstOrDefault(c => c.Type == "status")?.Value;

    public IReadOnlyList<string> Roles
    {
        get
        {
            string? rolesJson = _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(rolesJson))
            {
                return Array.Empty<string>();
            }

            return JsonConvert.DeserializeObject<List<string>>(rolesJson) ?? new List<string>();
        }
    }
    public string? FirstName => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;

    public string? LastName => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

    public string? FullName => UserAccount.GetFullName(FirstName, LastName);
}
