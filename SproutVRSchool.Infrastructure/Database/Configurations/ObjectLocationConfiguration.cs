using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ObjectLocations;

namespace SproutVRSchool.Infrastructure.Database.Configurations;

internal sealed class ObjectLocationConfiguration : BaseEntityConfiguration<ObjectLocation>
{
    public override void Configure(EntityTypeBuilder<ObjectLocation> builder)
    {
        // override the hasKey from BaseEntityConfiguration
        builder.Ignore(ol => ol.Id);
        base.Configure(builder);

        // Schema
        builder.ToTable("ObjectLocations", schema: AppCts.DB.APP_SCHEMA);

        // Composite Primary Key
        builder.HasKey(ol => new { ol.TaskLocationId, ol.ObjectId });

        // Foreign Keys
        builder.Property(ol => ol.ObjectId).IsRequired();
        builder.Property(ol => ol.TaskLocationId).IsRequired();

        // Indexing

        // Properties        

        // Relationships
        builder.HasOne(ol => ol.TaskLocation)
               .WithMany(tl => tl.ObjectLocations)
               .HasForeignKey(ol => ol.TaskLocationId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ol => ol.MapObject)
               .WithMany(mo => mo.ObjectLocations)
               .HasForeignKey(ol => ol.ObjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
