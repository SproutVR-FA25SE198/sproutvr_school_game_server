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

public class SchoolServerDbContextSeeder
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
    public async Task SeedDevelopmentAsync()
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

        // seeding all tables
        await _dataSeeder.SeedAllTablesAsync();
    }

    /// <summary>
    /// Seedingh data for production mode only
    /// </summary>
    /// <returns></returns>
    public async Task SeedProductionAsync()
    {
        // add subsequent files to seed here
        _dataSeeder.AddRelativePath<MasterSubject>(AppCts.SeederFilePaths.MasterSubjectFilePath);
        _dataSeeder.AddRelativePath<Subject>(AppCts.SeederFilePaths.SubjectFilePath);
        _dataSeeder.AddRelativePath<VRDevice>(AppCts.SeederFilePaths.VRDeviceFilePath);

        // seeding all tables
        await _dataSeeder.SeedAllTablesAsync();
    }
}
