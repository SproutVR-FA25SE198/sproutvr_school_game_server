using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Maps;

namespace SproutVRSchool.Infrastructure.Configurations;

internal sealed class MapConfiguration : BaseEntityConfiguration<Map>
{
    public override void Configure(EntityTypeBuilder<Map> builder)
    {
        // Apply base configuration (Id, CreatedAtUtc, UpdatedAtUtc)
        base.Configure(builder);

        // Schema
        builder.ToTable("Maps", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys
        builder.Property(m => m.SubjectId).IsRequired();

        // Indexing
        builder.HasIndex(m => m.SubjectId);
        builder.HasIndex(m => m.MapCode);

        // Properties
        builder.Property(m => m.MapCode)
            .IsRequired();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.ImageUrl)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<EnumToStringConverter<MapStatus>>();

        // Relationships
        builder.HasOne(m => m.Subject)
               .WithMany(s => s.Maps)
               .HasForeignKey(m => m.SubjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.TaskLocations)
               .WithOne(tl => tl.Map)
               .HasForeignKey(tl => tl.MapId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MapObjects)
               .WithOne(mo => mo.Map)
               .HasForeignKey(mo => mo.MapId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
