using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRLessons;

namespace SproutVRSchool.Infrastructure.Database.Configurations.VRLessons;

internal sealed class VRLessonConfiguration : BaseEntityConfiguration<VRLesson>
{
    public override void Configure(EntityTypeBuilder<VRLesson> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("VRLessons", schema: AppCts.DB.APP_SCHEMA);

        // Foreign Keys
        builder.Property(vrl => vrl.LessonId).IsRequired();
        builder.Property(vrl => vrl.MapId).IsRequired();

        // Indexing
        builder.HasIndex(vrl => vrl.LessonId);
        builder.HasIndex(vrl => vrl.MapId);

        // Properties
        builder.Property(vrl => vrl.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vrl => vrl.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(vrl => vrl.MaxDuration)
            .IsRequired();

        builder.Property(vrl => vrl.PresetJsonUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(vrl => vrl.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(vrl => vrl.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<VRLessonStatus>>();

        // Relationships
        builder.HasOne(vrl => vrl.Lesson)
               .WithMany(l => l.VRLessons)
               .HasForeignKey(vrl => vrl.LessonId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vrl => vrl.Map)
               .WithMany()
               .HasForeignKey(vrl => vrl.MapId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(vrl => vrl.VRLearningSessions)
               .WithOne(vrls => vrls.VRLesson)
               .HasForeignKey(vrl => vrl.VRLessonId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
