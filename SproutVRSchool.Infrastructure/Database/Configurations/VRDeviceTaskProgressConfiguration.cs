using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;

namespace SproutVRSchool.Infrastructure.Database.Configurations;

internal sealed class VRDeviceTaskProgressConfiguration : BaseEntityConfiguration<VRDeviceTaskProgress>
{
    public override void Configure(EntityTypeBuilder<VRDeviceTaskProgress> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("VRDeviceTaskProgresses", schema: AppCts.DB.APP_SCHEMA);

        // Foreign Keys
        builder.Property(vrtp => vrtp.VRDeviceId).IsRequired();
        builder.Property(vrtp => vrtp.VRTaskId).IsRequired();
        builder.Property(vrtp => vrtp.VRLearningSessionId).IsRequired();

        // Indexing
        builder.HasIndex(vrtp => vrtp.VRDeviceId);
        builder.HasIndex(vrtp => vrtp.VRTaskId);
        builder.HasIndex(vrtp => vrtp.VRLearningSessionId);

        // Properties
        builder.Property(vrtp => vrtp.QuestionText)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(vrtp => vrtp.IsCorrect)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(vrtp => vrtp.AnswerText)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(vrtp => vrtp.CompletionTimeAtUtc)
            .IsRequired();

        // Relationships
        builder.HasOne(vrtp => vrtp.VRDevice)
               .WithMany(vrd => vrd.VRDeviceTaskProgresses)
               .HasForeignKey(vrtp => vrtp.VRDeviceId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vrtp => vrtp.VRTask)
               .WithMany(vrt => vrt.DeviceTaskProgresses)
               .HasForeignKey(vrtp => vrtp.VRTaskId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vrtp => vrtp.VRLearningSession)
               .WithMany(vrls => vrls.VRDeviceTaskProgresses)
               .HasForeignKey(vrtp => vrtp.VRLearningSessionId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
