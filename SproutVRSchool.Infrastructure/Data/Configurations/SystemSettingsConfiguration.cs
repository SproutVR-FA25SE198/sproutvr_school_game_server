using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.SystemSettings;

namespace SproutVRSchool.Infrastructure.Data.Configurations;

public sealed class SystemSettingsConfiguration : IEntityTypeConfiguration<SystemSetting>
{
    public void Configure(EntityTypeBuilder<SystemSetting> builder)
    {
        builder.ToTable("SystemSettings", schema: AppCts.Db.APP_SCHEMA);

        builder.HasKey(x => x.Key);

        builder.Property(x => x.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Value)
            .IsRequired(false);

        builder.Property(x => x.Description)
            .HasMaxLength(255)
            .IsRequired(false);
    }
}
