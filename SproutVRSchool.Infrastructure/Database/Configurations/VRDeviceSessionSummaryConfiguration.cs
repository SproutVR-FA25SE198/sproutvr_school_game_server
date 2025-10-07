using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;

namespace SproutVRSchool.Infrastructure.Database.Configurations;

internal sealed class VRDeviceSessionSummaryConfiguration : BaseEntityConfiguration<VRDeviceSessionSummary>
{
    public override void Configure(EntityTypeBuilder<VRDeviceSessionSummary> builder)
    {
        // override the hasKey from BaseEntityConfiguration
        builder.Ignore(vdss => vdss.Id);
        base.Configure(builder);

        // Schema
        builder.ToTable("VRDeviceSessionSummaries", schema: AppCts.DB.APP_SCHEMA);

        // Composite Primary Key
        builder.HasKey(vdss => new { vdss.VRLearningSessionId, vdss.VRDeviceId });

        // Foreign Keys
        builder.Property(vdss => vdss.VRDeviceId).IsRequired();
        builder.Property(vdss => vdss.VRLearningSessionId).IsRequired();

        // Indexing

        // Properties
        builder.Property(vdss => vdss.StudentName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vdss => vdss.NoTasksCompleted)
            .IsRequired();

        // Relationships
        builder.HasOne(vdss => vdss.VRLearningSession)
               .WithMany(vrls => vrls.VRDeviceSessionSummaries)
               .HasForeignKey(vdss => vdss.VRLearningSessionId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vdss => vdss.VRDevice)
               .WithMany(vrd => vrd.VRDeviceSessionSummaries)
               .HasForeignKey(vdss => vdss.VRDeviceId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
