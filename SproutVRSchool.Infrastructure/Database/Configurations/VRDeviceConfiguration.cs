using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Infrastructure.Database.Configurations.VRDevices;

internal sealed class VRDeviceConfiguration : BaseEntityConfiguration<VRDevice>
{
    public override void Configure(EntityTypeBuilder<VRDevice> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("VRDevices", schema: AppCts.DB.APP_SCHEMA);

        // Foreign Keys

        // Indexing
        builder.HasIndex(vrd => vrd.SerialNumber).IsUnique();

        // Properties
        builder.Property(vrd => vrd.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vrd => vrd.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vrd => vrd.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<VRDeviceStatus>>();

        // Relationships
        builder.HasMany(vrd => vrd.VRDeviceSessionSummaries)
               .WithOne()
               .HasForeignKey(vrds => vrds.VRDeviceId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(vrd => vrd.VRDeviceTaskProgresses)
               .WithOne()
               .HasForeignKey(vrdt => vrdt.VRDeviceId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
