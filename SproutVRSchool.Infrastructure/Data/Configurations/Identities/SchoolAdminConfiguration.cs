using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Data.Configurations.Identities;

internal sealed class SchoolAdminConfiguration : IEntityTypeConfiguration<SchoolAdmin>
{
    public void Configure(EntityTypeBuilder<SchoolAdmin> builder)
    {
        // Schema
        builder.ToTable("SchoolAdmins", schema: AppCts.Db.AUTH_SCHEMA);

        // Indexing
        builder.HasIndex(sa => sa.OrganizationId);

        // Properties
        builder.Property(sa => sa.OrganizationId)
               .IsRequired();
    }
}
