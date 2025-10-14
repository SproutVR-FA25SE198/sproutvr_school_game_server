using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Data.Configurations.Identities;

internal sealed class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // Schema
        builder.ToTable("Teachers", schema: AppCts.Db.AUTH_SCHEMA);

        // Indexing

        // Properties

        // Relationships
        builder.HasMany(t => t.Lessons)
            .WithOne()
            .HasForeignKey(t => t.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.VRLearningSessions)
            .WithOne()
            .HasForeignKey(t => t.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
