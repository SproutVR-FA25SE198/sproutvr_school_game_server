using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Infrastructure.Extensions;
using SproutVRSchool.Presentation.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ======================================
// === Built-in services
// ======================================

builder.Services.AddControllers();

// ======================================
// === User-defined services
// ======================================

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

// ======================================
// === Middlewares
// ======================================

WebApplication app = builder.Build();
IWebHostEnvironment env = app.Services.GetRequiredService<IWebHostEnvironment>();
IConfigurationSection miscConfigs = app.Configuration.GetSection("Miscs");

if (env.IsDevelopment())
{
    app.ApplyDatabaseDrop(miscConfigs.GetValue<bool?>("IsDroppingDatabaseOnStartup") ?? false);
    app.ApplyDatabaseMigrations();
    await app.ApplySeedingDevelopment();
}
else if (env.IsProduction())
{
    app.ApplyDatabaseMigrations();
    await app.ApplySeedingProduction();
}

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);
