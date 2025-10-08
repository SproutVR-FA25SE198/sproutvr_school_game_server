using Microsoft.EntityFrameworkCore;
using SproutVRSchool.Infrastructure.Data;
using SproutVRSchool.Infrastructure.Data.Seeders;

namespace SproutVRSchool.Presentation.Extensions;

internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Apply the database migrations.
    /// </summary>
    /// <param name="app"></param>
    public static void ApplyDatabaseMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        SchoolServerDbContext dbContext = scope.ServiceProvider.GetRequiredService<SchoolServerDbContext>();
        dbContext.Database.Migrate();
    }

    /// <summary>
    /// Apply the database drop. This is mainly for testing purposes.
    /// </summary>
    /// <param name="app"></param>
    public static void ApplyDatabaseDrop(this IApplicationBuilder app, bool IsDroppingDatabaseOnStartup = false)
    {
        if (!IsDroppingDatabaseOnStartup)
        {
            return;
        }
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        SchoolServerDbContext dbContext = scope.ServiceProvider.GetRequiredService<SchoolServerDbContext>();
        dbContext.Database.EnsureDeleted();
    }

    /// <summary>
    /// Seeding data for development environment.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task ApplySeedingDevelopment(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        SchoolServerDbContextSeeder seeder = scope.ServiceProvider.GetRequiredService<SchoolServerDbContextSeeder>();
        await seeder.SeedDevelopmentAsync();
    }

    /// <summary>
    /// Seeding data for production environment.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task ApplySeedingProduction(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        SchoolServerDbContextSeeder seeder = scope.ServiceProvider.GetRequiredService<SchoolServerDbContextSeeder>();
        await seeder.SeedProductionAsync();
    }
}
