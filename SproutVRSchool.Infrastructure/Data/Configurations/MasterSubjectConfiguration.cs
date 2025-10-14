using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.MasterSubjects;

namespace SproutVRSchool.Infrastructure.Data.Configurations;

internal sealed class MasterSubjectConfiguration : BaseEntityConfiguration<MasterSubject>
{
    public override void Configure(EntityTypeBuilder<MasterSubject> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("MasterSubjects", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys

        // Indexing

        // Properties
        builder.Property(ms => ms.Name)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(100);

        builder.Property(ms => ms.Description)
            .IsRequired()
            .HasColumnType("text")
            .HasMaxLength(1000);

        builder.Property(ms => ms.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(ms => ms.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<MasterSubjectStatus>>();

        // Relationships
        builder.HasMany(ms => ms.Subjects)
               .WithOne()
               .HasForeignKey(s => s.MasterSubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
