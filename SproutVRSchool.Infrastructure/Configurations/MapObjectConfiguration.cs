using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.MapObjects;

namespace SproutVRSchool.Infrastructure.Configurations;

internal sealed class MapObjectConfiguration : BaseEntityConfiguration<MapObject>
{
    public override void Configure(EntityTypeBuilder<MapObject> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("MapObjects", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys
        builder.Property(mo => mo.MapId).IsRequired();

        // Indexing

        // Properties
        builder.Property(mo => mo.Name)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(100);

        builder.Property(mo => mo.ObjectCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mo => mo.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        // Relationships
        builder.HasOne(mo => mo.Map)
               .WithMany(m => m.MapObjects)
               .HasForeignKey(mo => mo.MapId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(mo => mo.ObjectActivityTypes)
               .WithOne(oat => oat.MapObject)
               .HasForeignKey(oa => oa.MapObjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
