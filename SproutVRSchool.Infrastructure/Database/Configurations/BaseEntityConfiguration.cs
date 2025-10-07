using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SproutVRSchool.Domain.Entities;

namespace SproutVRSchool.Infrastructure.Database.Configurations;

internal class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.CreateAtUtc)
               .IsRequired()
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

        builder.Property(e => e.UpdateAtUtc)
               .IsRequired()
               .ValueGeneratedOnAddOrUpdate()
               .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
    }
}
