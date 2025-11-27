using System.Globalization;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Domain.Entities.Identities;

public sealed class Teacher : UserAccount
{
    // navigation properties
    public ICollection<Lesson> Lessons { get; set; } = [];
    public ICollection<VRLearningSession> VRLearningSessions { get; set; } = [];

    public static Teacher Create(string username, string email, string firstName, string lastName, string dateOfBirth)
    {
        return new Teacher()
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = DateOnly.ParseExact(dateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Status = UserAccountStatus.Active,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };
    }
}

