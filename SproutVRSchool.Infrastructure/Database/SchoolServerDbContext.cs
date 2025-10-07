using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Lessons;

namespace SproutVRSchool.Infrastructure.Database;

internal sealed class SchoolServerDbContext : IdentityDbContext<UserAccount, UserAccountRole, Guid>
{
    // =============================
    // ==== DbSets
    // =============================

    // Others
    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    // Identity
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<SchoolAdmin> SchoolAdmins { get; set; }

    // =============================
    // ==== Constructors
    // =============================

    public SchoolServerDbContext(DbContextOptions options) : base(options)
    {
    }

    // =============================
    // ==== Methods
    // =============================

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Set the default schema for the Identity tables
        builder.HasDefaultSchema(AppCts.DB.AUTH_SCHEMA);

        // Apply all configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(typeof(SchoolServerDbContext).Assembly);
    }
}
