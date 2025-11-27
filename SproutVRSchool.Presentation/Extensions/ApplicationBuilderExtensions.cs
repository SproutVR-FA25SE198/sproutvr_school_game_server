using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Domain;
using SproutVRSchool.Infrastructure.Data;

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
    public static void ApplyDatabaseDrop(this IApplicationBuilder app)
    {
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
        ISchoolServerDbContextSeeder seeder = scope.ServiceProvider.GetRequiredService<ISchoolServerDbContextSeeder>();
        IIdentityDbContextSeeder identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentityDbContextSeeder>();

        await identitySeeder.SeedDevelopmentAsync();
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
        ISchoolServerDbContextSeeder seeder = scope.ServiceProvider.GetRequiredService<ISchoolServerDbContextSeeder>();
        IIdentityDbContextSeeder identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentityDbContextSeeder>();

        await identitySeeder.SeedProductionAsync();
        await seeder.SeedProductionAsync();
    }

    /// <summary>
    /// Apply the Static File middleware
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static void ApplyStaticMiddleware(this IApplicationBuilder app)
    {
        // "C:\ProgramData\SproutVRSchool\Content" in Development Mode
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        string contenRootPath = configuration.GetValue<string>("FileLocalStorageSettings:ContentRootPath")
            ?? throw new InvalidOperationException("FileStorageSettings:ContentRootPath is not configured.");

        Directory.CreateDirectory(contenRootPath);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(contenRootPath),
            RequestPath = $"/api{AppCts.Files.PREFIX_PUBLIC_CONTENT_PATH}" // e.g. /api/content
        });
    }
}
