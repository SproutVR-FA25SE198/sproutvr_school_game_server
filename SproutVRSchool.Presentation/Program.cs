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

#pragma warning disable S125 // Sections of code should not be commented out
    await app.ApplySeedingDevelopment();
    app.MapGrpcReflectionService().AllowAnonymous();
#pragma warning restore S125 // Sections of code should not be commented out
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
app.UseRouting();

// Controller
app.MapControllers();

// Grpc Services
app.MapGrpcService<GrpcTeacherVRLearningSessionService>();
app.MapGrpcService<GrpcVRGlassVRLearningSessionService>();

app.MapGet("/", () => "gRPC Server is running.");

await app.RunAsync();
