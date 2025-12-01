using MediatR;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.ChangePassword;

public sealed record AuthChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmNewPassword)
    : IRequest<AuthChangePasswordCommandResponseDto>
{
}
