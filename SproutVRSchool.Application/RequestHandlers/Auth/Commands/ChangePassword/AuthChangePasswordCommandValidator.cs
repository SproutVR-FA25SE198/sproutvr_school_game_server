using System.Text.RegularExpressions;
using FluentValidation;
using SproutVRSchool.Application.Exceptions.Accounts;

namespace SproutVRSchool.Application.RequestHandlers.Auth.Commands.ChangePassword;

public sealed class AuthChangePasswordCommandValidator : AbstractValidator<AuthChangePasswordCommand>
{
    // Regex enforce: Min 8 chars, 1 uppercase, 1 lowercase, 1 number, and NO special characters.
    private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9]{8,}$";

    public AuthChangePasswordCommandValidator()
    {
        // 1. Current Password Check
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.");

        // 2. New Password
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .Must(newPassword => Regex.IsMatch(newPassword, PasswordPattern))
            .WithMessage("Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number, and NO special characters.")
            .WithErrorCode(nameof(SvrInvalidPasswordFormatException));

        // 3. Confirmation Password
        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
            .WithMessage("Confirmation password is required.")
            .Equal(x => x.NewPassword)
            .WithMessage("New password and confirmation password do not match.")
            .WithErrorCode(nameof(SvrPasswordMismatchException));
    }
}
