using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.Abstractions.AccountServices;

public interface ITokenService
{
    /// <summary>
    /// Generates an authentication token for the specified user account with assigned roles, valid for the given
    /// duration in Vietnam time.
    /// </summary>
    /// <remarks>The expiration time is calculated based on Vietnam's time zone (UTC+7). The token grants
    /// access according to the specified roles until it expires.</remarks>
    /// <param name="user">The user account for which the token is generated. Cannot be null.</param>
    /// <param name="role">A list of roles to associate with the token. Each role defines the user's permissions within the system. Cannot
    /// be null or contain null entries.</param>
    /// <param name="durationInMinutes">The duration for which the token remains valid, expressed as a <see cref="TimeSpan"/> representing minutes. Must
    /// be greater than zero.</param>
    /// <returns>A tuple containing the generated token as a string and the expiration time in Vietnam time as a <see
    /// cref="DateTimeOffset"/>.</returns>
    (string Token, DateTimeOffset ExpiredAtVietNam) GenerateToken(UserAccount user, IList<string> role, TimeSpan durationInMinutes);

    /// <summary>
    /// Manullay Validate Token, could integrate it in the middleware
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<ClaimsPrincipal?> ValidateTokenAsync(string token);
}
