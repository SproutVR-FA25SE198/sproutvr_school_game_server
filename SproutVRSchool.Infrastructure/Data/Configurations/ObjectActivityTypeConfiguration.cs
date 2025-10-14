using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ObjectActivityTypes;

namespace SproutVRSchool.Infrastructure.Data.Configurations;

internal sealed class ObjectActivityTypeConfiguration : BaseEntityConfiguration<ObjectActivityType>
{
    public override void Configure(EntityTypeBuilder<ObjectActivityType> builder)
    {
        // override the hasKey from BaseEntityConfiguration first then run the base class
        base.Configure(builder);
        builder.Ignore(ol => ol.Id);

        // Schema
        builder.ToTable("ObjectActivityTypes", schema: AppCts.Db.APP_SCHEMA);

        // Composite Primary Key
        builder.HasKey(oat => new { oat.MapObjectId, oat.ActivityTypeId });

        // Foreign Keys
        builder.Property(oat => oat.MapObjectId).IsRequired();
        builder.Property(oat => oat.ActivityTypeId).IsRequired();

        // Indexing

        // Properties

        // Relationships
        builder.HasOne(oat => oat.MapObject)
               .WithMany(mo => mo.ObjectActivityTypes)
               .HasForeignKey(oat => oat.MapObjectId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oat => oat.ActivityType)
               .WithMany(at => at.ObjectActivityTypes)
               .HasForeignKey(oat => oat.ActivityTypeId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
    }
}
