using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.MasterSubjects;
using SproutVRSchool.Domain.Entities.ObjectActivityTypes;
using SproutVRSchool.Domain.Entities.ObjectLocations;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.TaskLocations;
using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Infrastructure.Data;

public sealed class SchoolServerDbContext : IdentityDbContext<UserAccount, UserAccountRole, Guid>, ISchoolServerDbContext
{
    // =============================
    // ==== DbSets
    // =============================

    // Others
    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<MapObject> MapObjects { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<MasterSubject> MasterSubjects { get; set; }
    public DbSet<ObjectActivityType> ObjectActivityTypes { get; set; }
    public DbSet<ObjectLocation> ObjectLocations { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TaskLocation> TaskLocations { get; set; }
    public DbSet<VRDevice> VRDevices { get; set; }
    public DbSet<VRDeviceSessionSummary> VRDeviceSessionSummaries { get; set; }
    public DbSet<VRDeviceTaskProgress> VRDeviceTaskProgresses { get; set; }
    public DbSet<VRLearningSession> VRLearningSessions { get; set; }
    public DbSet<VRLesson> VRLessons { get; set; }
    public DbSet<VRTask> VRTasks { get; set; }

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

    /// <summary>
    /// On creating models
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Define a custom case-insensitive collation using ICU
        builder.HasPostgresExtension("citext");

        // Set the default schema for the Identity tables
        builder.HasDefaultSchema(AppCts.Db.AUTH_SCHEMA);

        // Apply all configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(typeof(SchoolServerDbContext).Assembly);
    }
}
