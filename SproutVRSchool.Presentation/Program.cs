using SproutVRSchool.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ======================================
// === Built-in services
// ======================================

builder.Services.AddControllers();

// ======================================
// === User-defined services
// ======================================

builder.Services.AddSchoolServerDbContext(builder.Configuration);

WebApplication app = builder.Build();

// ======================================
// === Middlewares
// ======================================

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);
