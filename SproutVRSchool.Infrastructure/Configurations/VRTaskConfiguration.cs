using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Infrastructure.Configurations;

internal sealed class VRTaskConfiguration : BaseEntityConfiguration<VRTask>
{
    public override void Configure(EntityTypeBuilder<VRTask> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("VRTasks", schema: AppCts.Db.APP_SCHEMA);

        // Foreign Keys
        builder.Property(vrt => vrt.TaskLocationId).IsRequired();
        builder.Property(vrt => vrt.MapObjectId).IsRequired();
        builder.Property(vrt => vrt.ActivityTypeId).IsRequired();

        // Indexing
        builder.HasIndex(vrt => vrt.TaskLocationId);
        builder.HasIndex(vrt => vrt.MapObjectId);
        builder.HasIndex(vrt => vrt.ActivityTypeId);

        // Properties
        builder.Property(vrt => vrt.TaskNumber)
            .HasColumnType("citext")
            .IsRequired();

        builder.Property(vrt => vrt.Description)
            .HasColumnType("text")
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(vrt => vrt.TaskLocation)
               .WithMany(tl => tl.VRTasks)
               .HasForeignKey(vrt => vrt.TaskLocationId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vrt => vrt.MapObject)
               .WithMany(mo => mo.VRTasks)
               .HasForeignKey(vrt => vrt.MapObjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vrt => vrt.ActivityType)
               .WithMany(at => at.VRTasks)
               .HasForeignKey(vrt => vrt.ActivityTypeId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(vrt => vrt.DeviceTaskProgresses)
               .WithOne(dtp => dtp.VRTask)
               .HasForeignKey(dtp => dtp.VRTaskId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
