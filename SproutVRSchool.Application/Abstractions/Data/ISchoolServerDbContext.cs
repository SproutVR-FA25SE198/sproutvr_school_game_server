using Microsoft.EntityFrameworkCore;
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

namespace SproutVRSchool.Application.Abstractions.Data;

public interface ISchoolServerDbContext
{
    // =============================
    // ==== DbSets
    // =============================

    // Others
    DbSet<ActivityType> ActivityTypes { get; set; }
    DbSet<Lesson> Lessons { get; set; }
    DbSet<MapObject> MapObjects { get; set; }
    DbSet<Map> Maps { get; set; }
    DbSet<MasterSubject> MasterSubjects { get; set; }
    DbSet<ObjectActivityType> ObjectActivityTypes { get; set; }
    DbSet<ObjectLocation> ObjectLocations { get; set; }
    DbSet<Subject> Subjects { get; set; }
    DbSet<TaskLocation> TaskLocations { get; set; }
    DbSet<VRDevice> VRDevices { get; set; }
    DbSet<VRDeviceSessionSummary> VRDeviceSessionSummaries { get; set; }
    DbSet<VRDeviceTaskProgress> VRDeviceTaskProgresses { get; set; }
    DbSet<VRLearningSession> VRLearningSessions { get; set; }
    DbSet<VRLesson> VRLessons { get; set; }
    DbSet<VRTask> VRTasks { get; set; }

    // Identity
    DbSet<UserAccount> UserAccounts { get; set; }
    DbSet<Teacher> Teachers { get; set; }
    DbSet<SchoolAdmin> SchoolAdmins { get; set; }
}
