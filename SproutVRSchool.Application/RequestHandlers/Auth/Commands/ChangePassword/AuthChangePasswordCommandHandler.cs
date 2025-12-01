using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SproutVRSchool.Application.Abstractions.AccountServices;
using SproutVRSchool.Application.Exceptions.Accounts;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.ChangePassword;

public sealed class AuthChangePasswordCommandHandler(
    ICurrentLoggedInUserAccountService currentUser,
    UserManager<UserAccount> userManager) : IRequestHandler<AuthChangePasswordCommand, AuthChangePasswordCommandResponseDto>
{
    public async Task<AuthChangePasswordCommandResponseDto> Handle(AuthChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if the current user is authenticated or not 
        if (!currentUser.IsAuthenticated)
        {
            throw new SvrUnauthorizedAccessException("You must be logged in to perform this action.");
        }

        // 2. If the current account is disabled, throw an unauthorized access exception
        if (currentUser == null
            || currentUser.Status == null
            || currentUser.Status == UserAccountStatus.Disabled.ToString())
        {
            throw new SvrSelfAccountStatusChangeException("Your account has been disabled. Please contact support for assistance.");
        }

        // 3. Throw exception if dont have user id
        if (currentUser.UserId == null || currentUser.UserId == Guid.Empty)
        {
            throw new SvrUnauthorizedAccessException("Invalid user identity.");
        }

        // 4. Check Complexity (Plsae put this inside the validator)
        // ^             Start of string
        // (?=.*[a-z])   At least one lowercase
        // (?=.*[A-Z])   At least one uppercase
        // (?=.*\d)      At least one number
        // [a-zA-Z0-9]   Allowed characters (Alphanumeric ONLY, No Special Chars)
        // {8,}          Min length 8
        // $             End of string

        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]{8,}$";
        if (!Regex.IsMatch(request.NewPassword, passwordPattern))
        {
            throw new SvrInvalidPasswordFormatException(
                "Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number, and NO special characters.");
        }

        // 5. Retrieve user from db and update password
        string userId = currentUser.UserId.Value.ToString();
        UserAccount user = await userManager.FindByIdAsync(userId)
             ?? throw new SvrUnauthorizedAccessException("User account not found.");

        IdentityResult result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        // 6. If succeeded, then return new, else return false 
        if (!result.Succeeded)
        {
            throw new SvrResourceValidationException(result.Errors.Select(e => new SvrResourceValidationError(e.Code, e.Description)));
        }

        return new AuthChangePasswordCommandResponseDto(true);
    }
}
