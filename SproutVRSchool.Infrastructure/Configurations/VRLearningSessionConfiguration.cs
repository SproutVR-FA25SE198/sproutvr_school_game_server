using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRLearningSessions;

namespace SproutVRSchool.Infrastructure.Configurations;

internal sealed class VRLearningSessionConfiguration : BaseEntityConfiguration<VRLearningSession>
{
    public override void Configure(EntityTypeBuilder<VRLearningSession> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("VRLearningSessions", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys
        builder.Property(vrls => vrls.VRLessonId).IsRequired();
        builder.Property(vrls => vrls.TeacherId).IsRequired();

        // Indexing
        builder.HasIndex(vrls => vrls.VRLessonId);
        builder.HasIndex(vrls => vrls.TeacherId);

        // Properties
        builder.Property(vrls => vrls.StartTime)
            .IsRequired();

        builder.Property(vrls => vrls.EndTime)
            .IsRequired();

        builder.Property(vrls => vrls.Duration)
            .IsRequired();

        builder.Property(vrls => vrls.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<VRLearningSessionStatus>>();

        // Relationships
        builder.HasOne(vrls => vrls.VRLesson)
               .WithMany(vrl => vrl.VRLearningSessions)
               .HasForeignKey(vrls => vrls.VRLessonId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vrls => vrls.Teacher)
               .WithMany(t => t.VRLearningSessions)
               .HasForeignKey(vrls => vrls.TeacherId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(vrls => vrls.VRDeviceTaskProgresses)
               .WithOne(vrtp => vrtp.VRLearningSession)
               .HasForeignKey(vrtp => vrtp.VRLearningSessionId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(vrls => vrls.VRDeviceSessionSummaries)
               .WithOne(vdss => vdss.VRLearningSession)
               .HasForeignKey(vdss => vdss.VRLearningSessionId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
