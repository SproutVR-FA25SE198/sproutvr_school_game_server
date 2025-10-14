namespace SproutVRSchool.Domain.Entities.Identities;

public sealed class SchoolAdmin : UserAccount
{
    public Guid OrganizationId { get; set; }
}
