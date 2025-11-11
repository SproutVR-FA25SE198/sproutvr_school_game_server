using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Commons.Responses;
using SproutVRSchool.Application.Exceptions.Resources;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.Accounts.Commands.AssignAccountStatus;

public sealed class SAAssignAccountStatusCommandHandler(
    UserManager<UserAccount> userManager,
    ILogger<SAAssignAccountStatusCommandHandler> logger
    )
    : IRequestHandler<SAAssignAccountStatusCommand, SAAssignAccountStatusCommandResponseDto>
{
    public async Task<SAAssignAccountStatusCommandResponseDto> Handle(
        SAAssignAccountStatusCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find existing user or throw
        UserAccount? user = await userManager.FindByIdAsync(request.UserId.ToString())
            ?? throw new SvrResourceNotFoundException($"User with ID {request.UserId} not found.");

        // 2. Assign status
        user.Status = request.Status;

        // 3. Save changes
        await userManager.UpdateAsync(user);

        // 4. Log
        logger.LogInformation("User '{AccountName}' status updated to {Status}", user.GetFullName(), user.Status);

        // 5. Return response
        return new SAAssignAccountStatusCommandResponseDto(
            UserId: request.UserId,
            AccountName: user.GetFullName(),
            Message: $"Status {request.Status} assigned successfully.",
            Status: new StatusDto(user.Status)
        );
    }
}
