using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Configurations.Identities;

internal sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        // Schema
        builder.ToTable("UserAccounts", schema: AppCts.DB.AUTH_SCHEMA);

        // Indexing
        builder.HasIndex(ua => ua.Email);

        // Properties
        builder.HasKey(ua => ua.Id);
        builder.Property(ua => ua.Id)
            .ValueGeneratedNever();

        builder.Property(ua => ua.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ua => ua.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ua => ua.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ua => ua.Status)
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<UserAccountStatus>>();

        builder.Property(ua => ua.DateOfBirth)
            .IsRequired(false);

        builder.Property(ua => ua.UserName)
            .IsRequired()
            .HasMaxLength(50);

        // handle at application level
        builder.Property(ua => ua.CreatedAtUtc)
            .IsRequired()
            .ValueGeneratedNever();

        // handle at application level
        builder.Property(ua => ua.UpdatedAtUtc)
            .IsRequired()
            .ValueGeneratedNever();
    }
}
