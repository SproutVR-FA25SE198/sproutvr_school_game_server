using Microsoft.AspNetCore.Identity;

namespace SproutVRSchool.Domain.Entities.Identities;

public class UserAccount : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public UserAccountStatus Status { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

    public string GetFullName()
    {
        if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
        {
            return string.Empty;
        }
        else if (string.IsNullOrWhiteSpace(FirstName))
        {
            return LastName;
        }
        else if (string.IsNullOrWhiteSpace(LastName))
        {
            return FirstName;
        }
        else
        {
            return $"{FirstName} {LastName}";
        }
    }

    public static string GetFullName(string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
        {
            return string.Empty;
        }
        else if (string.IsNullOrWhiteSpace(firstName))
        {
            return lastName;
        }
        else if (string.IsNullOrWhiteSpace(lastName))
        {
            return firstName;
        }
        else
        {
            return $"{firstName} {lastName}";
        }
    }
}


