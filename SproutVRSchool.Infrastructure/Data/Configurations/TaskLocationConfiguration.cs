using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.TaskLocations;

namespace SproutVRSchool.Infrastructure.Data.Configurations;

internal sealed class TaskLocationConfiguration : BaseEntityConfiguration<TaskLocation>
{
    public override void Configure(EntityTypeBuilder<TaskLocation> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("TaskLocations", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys
        builder.Property(tl => tl.MapId).IsRequired();

        // Indexing
        builder.HasIndex(tl => tl.MapId);

        // Properties
        builder.Property(tl => tl.LocationCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tl => tl.Name)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(100);

        builder.Property(tl => tl.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);

        // Relationships
        builder.HasOne(tl => tl.Map)
               .WithMany(m => m.TaskLocations)
               .HasForeignKey(tl => tl.MapId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tl => tl.VRTasks)
               .WithOne(vrt => vrt.TaskLocation)
               .HasForeignKey(vrt => vrt.TaskLocationId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(tl => tl.ObjectLocations)
               .WithOne(ol => ol.TaskLocation)
               .HasForeignKey(ol => ol.TaskLocationId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
