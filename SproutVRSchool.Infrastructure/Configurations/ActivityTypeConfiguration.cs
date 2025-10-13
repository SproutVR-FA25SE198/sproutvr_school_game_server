using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ActivityTypes;

namespace SproutVRSchool.Infrastructure.Configurations;

internal sealed class ActivityTypeConfiguration : BaseEntityConfiguration<ActivityType>
{
    public override void Configure(EntityTypeBuilder<ActivityType> builder)
    {
        base.Configure(builder);

        // Schema
        builder.ToTable("ActivityTypes", schema: AppCts.Db.APP_SCHEMA);

        // Indexing

        // Properties
        builder.Property(at => at.Name)
            .IsRequired()
            .HasColumnType("citext")
            .HasMaxLength(100);

        builder.Property(at => at.ActivityCode)
            .IsRequired()
            .HasMaxLength(100);

        // Relationships
        builder.HasMany(at => at.ObjectActivityTypes)
               .WithOne()
               .HasForeignKey(oa => oa.ActivityTypeId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(at => at.VRTasks)
               .WithOne()
               .HasForeignKey(t => t.ActivityTypeId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
