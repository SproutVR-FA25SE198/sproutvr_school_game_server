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
}


