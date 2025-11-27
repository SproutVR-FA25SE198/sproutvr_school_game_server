using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.MasterSubjects;
using SproutVRSchool.Domain.Entities.ObjectActivityTypes;
using SproutVRSchool.Domain.Entities.ObjectLocations;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.TaskLocations;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Infrastructure.Data.Seeders;

public class SchoolServerDbContextSeeder : ISchoolServerDbContextSeeder
{
    // ===========================
    // === Fields
    // ===========================
    private readonly IDataSeeder _dataSeeder;

    // ===========================
    // === Constructors
    // ===========================

    public SchoolServerDbContextSeeder(
    IDataSeeder dataSeeder)
    {
        _dataSeeder = dataSeeder;
    }

    // ===========================
    // === Methods
    // ===========================


    /// <summary>
    /// Seeding all data in development mode
    /// </summary>
    /// <returns></returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task SeedDevelopmentAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable S125 // Sections of code should not be commented out
    {
        // add subsequent files to seed here
        _dataSeeder.AddRelativePath<MasterSubject>(AppCts.SeederFilePaths.MasterSubjectFilePath);
        _dataSeeder.AddRelativePath<Subject>(AppCts.SeederFilePaths.SubjectFilePath);
        _dataSeeder.AddRelativePath<ActivityType>(AppCts.SeederFilePaths.ActivityTypeFilePath);
        _dataSeeder.AddRelativePath<Map>(AppCts.SeederFilePaths.MapFilePath);
        _dataSeeder.AddRelativePath<MapObject>(AppCts.SeederFilePaths.MapObjectFilePath);
        _dataSeeder.AddRelativePath<TaskLocation>(AppCts.SeederFilePaths.TaskLocationFilePath);
        _dataSeeder.AddRelativePath<ObjectActivityType>(AppCts.SeederFilePaths.ObjectActivityTypeFilePath);
        _dataSeeder.AddRelativePath<ObjectLocation>(AppCts.SeederFilePaths.ObjectLocationFilePath);
        _dataSeeder.AddRelativePath<VRDevice>(AppCts.SeederFilePaths.VRDeviceFilePath);
        //_dataSeeder.AddRelativePath<Lesson>(AppCts.SeederFilePaths.LessonFilePath);
        //_dataSeeder.AddRelativePath<VRLesson>(AppCts.SeederFilePaths.VRLessonFilePath);

        //// seeding all tables
        await _dataSeeder.SeedAllTablesAsync();
    }
#pragma warning restore S125 // Sections of code should not be commented out

    /// <summary>
    /// Seedingh data for production mode only
    /// </summary>
    /// <returns></returns>
#pragma warning disable S4144 // Methods should not have identical implementations
    public async Task SeedProductionAsync()
#pragma warning restore S4144 // Methods should not have identical implementations
    {
        // add subsequent files to seed here
        _dataSeeder.AddRelativePath<MasterSubject>(AppCts.SeederFilePaths.MasterSubjectFilePath);
        _dataSeeder.AddRelativePath<Subject>(AppCts.SeederFilePaths.SubjectFilePath);
        _dataSeeder.AddRelativePath<ActivityType>(AppCts.SeederFilePaths.ActivityTypeFilePath);
        _dataSeeder.AddRelativePath<VRDevice>(AppCts.SeederFilePaths.VRDeviceFilePath);

        // seeding all tables
        await _dataSeeder.SeedAllTablesAsync();
    }
}
