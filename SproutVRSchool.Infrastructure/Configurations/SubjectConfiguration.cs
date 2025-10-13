using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.Subjects;

namespace SproutVRSchool.Infrastructure.Configurations;

internal sealed class SubjectConfiguration : BaseEntityConfiguration<Subject>
{
    public override void Configure(EntityTypeBuilder<Subject> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("Subjects", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys
        builder.Property(s => s.MasterSubjectId).IsRequired();

        // Indexing
        builder.HasIndex(s => s.MasterSubjectId);
        builder.HasIndex(s => s.Name).IsUnique();

        // Properties
        builder.Property(s => s.Name)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .IsRequired()
            .HasColumnType("text")
            .HasMaxLength(1000);

        builder.Property(s => s.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<SubjectStatus>>();

        // Relationships
        builder.HasOne(s => s.MasterSubject)
               .WithMany(ms => ms.Subjects)
               .HasForeignKey(s => s.MasterSubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Maps)
               .WithOne(m => m.Subject)
               .HasForeignKey(m => m.SubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Lessons)
               .WithOne(m => m.Subject)
               .HasForeignKey(m => m.SubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
