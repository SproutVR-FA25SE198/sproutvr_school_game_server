using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Infrastructure.Database.Configurations;

internal sealed class LessonConfiguration : BaseEntityConfiguration<Lesson>
{
    public override void Configure(EntityTypeBuilder<Lesson> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("Lessons", schema: AppCts.DB.APP_SCHEMA);

        // Foreign Keys
        builder.Property(l => l.SubjectId).IsRequired();
        builder.Property(l => l.TeacherId).IsRequired();

        // Indexing
        builder.HasIndex(l => l.Name);
        builder.HasIndex(l => l.TeacherId);
        builder.HasIndex(l => l.SubjectId);

        // Properties
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(l => l.ResourceUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(l => l.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<LessonStatus>>();

        // Relationships
        builder.HasOne(l => l.Subject)
               .WithMany()
               .HasForeignKey(l => l.SubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Teacher)
               .WithMany()
               .HasForeignKey(l => l.TeacherId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.VRLessons)
               .WithOne()
               .HasForeignKey(vr => vr.LessonId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
