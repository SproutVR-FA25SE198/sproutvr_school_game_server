using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Configurations.Identities;

internal sealed class SchoolAdminConfiguration : IEntityTypeConfiguration<SchoolAdmin>
{
    public void Configure(EntityTypeBuilder<SchoolAdmin> builder)
    {
        // Schema
        builder.ToTable("SchoolAdmins", schema: AppCts.DB.AUTH_SCHEMA);

        // Indexing
        builder.HasIndex(sa => sa.OrganizationId);

        // Properties
        builder.Property(sa => sa.OrganizationId)
               .IsRequired();
    }
}
