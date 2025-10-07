using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace SproutVRSchool.Infrastructure.Database;

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


        // seeding all tables
        await _dataSeeder.SeedAllTablesAsync();
    }
}
