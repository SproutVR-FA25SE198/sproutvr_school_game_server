using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Infrastructure.Extensions;
using SproutVRSchool.Presentation.Extensions;
using SproutVRSchool.Presentation.Grpc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ======================================
// === User-defined services
// ======================================

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation(builder.Configuration);

// ======================================
// === Middlewares
// ======================================

WebApplication app = builder.Build();
IWebHostEnvironment env = app.Services.GetRequiredService<IWebHostEnvironment>();
IConfigurationSection miscConfigs = app.Configuration.GetSection("Miscs");
bool IsDroppingDatabaseOnStartup = miscConfigs.GetValue<bool?>("IsSeedingDatabaseOnStartupDevelopment") ?? false;

if (env.IsDevelopment())
{
    app.ApplyDatabaseDrop();
    app.ApplyDatabaseMigrations();

    // only seed development data when the database is dropped
    if (IsDroppingDatabaseOnStartup)
    {
        await app.ApplySeedingDevelopment();
    }
    app.MapGrpcReflectionService().AllowAnonymous();
}
else if (env.IsProduction())
{
    app.ApplyDatabaseMigrations();
    await app.ApplySeedingProduction();
}

// Debugging gRPC
app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
    string.Join("\n", endpointSources.SelectMany(source => source.Endpoints.OfType<RouteEndpoint>().Select(e => e.RoutePattern.RawText))));

// Exception handlers
app.UseExceptionHandler();

// Static Files & Routing
app.ApplyStaticMiddleware();

// Cors
app.UseCors("DesktopAppPolicy");

// Determine endpoints
app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controller
app.MapControllers();

// Grpc Services
app.MapGrpcService<GrpcTeacherVRLearningSessionService>();
app.MapGrpcService<GrpcTeacherVRLearningSessionStateService>();
app.MapGrpcService<GrpcVRGlassVRLearningSessionService>();

app.MapGet("/", () => "gRPC Server is running.");

await app.RunAsync();
