using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Infrastructure.Data.Configurations;

internal sealed class LessonConfiguration : BaseEntityConfiguration<Lesson>
{
    public override void Configure(EntityTypeBuilder<Lesson> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("Lessons", schema: AppCts.Db.APP_SCHEMA);

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
            .HasColumnType("citext")
            .HasMaxLength(100);

        builder.Property(l => l.Description)
            .IsRequired()
            .HasColumnType("text")
            .HasMaxLength(1000);

        builder.Property(l => l.ResourceRelativeFilePath)
            .IsRequired(false)
            .HasMaxLength(300);

        builder.Property(l => l.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<LessonStatus>>();

        // Relationships
        builder.HasOne(l => l.Subject)
               .WithMany(s => s.Lessons)
               .HasForeignKey(l => l.SubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Teacher)
               .WithMany(t => t.Lessons)
               .HasForeignKey(l => l.TeacherId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.VRLessons)
               .WithOne(vrl => vrl.Lesson)
               .HasForeignKey(vr => vr.LessonId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
