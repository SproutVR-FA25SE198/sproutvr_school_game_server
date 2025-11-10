using MediatR;
using Microsoft.AspNetCore.Identity;
using SproutVRSchool.Application.Abstractions.AccountServices;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Exceptions.Accounts;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Queries.GetCurrentUser;

public sealed class AuthGetCurrentUserCommandHandler(
    ICurrentLoggedInUserAccountService currentUser,
    IDateTimeProvider dateTimeProvider,
    UserManager<UserAccount> userManager) : IRequestHandler<AuthGetCurrentUserCommand, AuthGetCurrentUserCommandResponseDto>
{
    public async Task<AuthGetCurrentUserCommandResponseDto> Handle(AuthGetCurrentUserCommand request, CancellationToken cancellationToken)
    {
        // 1. If the current user is not authenticated, throw an unauthorized access exception
        if (!currentUser.IsAuthenticated)
        {
            throw new SvrUnauthorizedAccessException("You must be logged in to perform this action.");
        }

        // 2. If the current account is disabled, throw an unauthorized access exception
        if (currentUser == null
            || currentUser.Status == null
            || currentUser.Status == UserAccountStatus.Disabled.ToString())
        {
            throw new SvrUnauthorizedAccessException("Your account has been disabled. Please contact support for assistance.");
        }

        // 3. Throw exception if dont have user id
        if (currentUser.UserId == null || currentUser.UserId == Guid.Empty)
        {
            throw new SvrUnauthorizedAccessException("Invalid user identity.");
        }

        // 4. Fetching information from database
        UserAccount user = await userManager.FindByIdAsync(currentUser.UserId.ToString() ?? "")
            ?? throw new SvrUnauthorizedAccessException("Invalid username or email.");

        IReadOnlyList<string> roles = currentUser.Roles;
        string email = user.Email ?? string.Empty;
        string fullName = user.GetFullName();
        string status = user.Status.ToString();
        string? organizationId = null;
        DateOnly? dateOfBirth = user.DateOfBirth;
        DateTimeOffset joinedAtUtc = user.CreatedAtUtc;
        DateTimeOffset joinedAtVietNam = dateTimeProvider.ConvertToVietNamTime(user.CreatedAtUtc);

        // 5. Get information based on School Admin
        if (currentUser.Roles.Contains(AppCts.Db.ROLE_SCHOOL_ADMIN))
        {
            organizationId = (user as Domain.Entities.Identities.SchoolAdmin)?.OrganizationId.ToString();
        }

        return new AuthGetCurrentUserCommandResponseDto(
            user.Id,
            email,
            fullName,
            status,
            roles,
            organizationId,
            dateOfBirth,
            joinedAtUtc,
            joinedAtVietNam
        );
    }
}

