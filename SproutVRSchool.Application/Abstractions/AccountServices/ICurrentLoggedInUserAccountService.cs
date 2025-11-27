namespace SproutVRSchool.Application.Abstractions.AccountServices;

public interface ICurrentLoggedInUserAccountService
{
    Guid? UserId { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
    string? FullName { get; }
    string? Status { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
}
