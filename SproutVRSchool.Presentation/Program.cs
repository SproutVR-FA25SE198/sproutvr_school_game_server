using SproutVRSchool.Application.Extensions;
using SproutVRSchool.Infrastructure.Extensions;
using SproutVRSchool.Presentation.Extensions;
using SproutVRSchool.Presentation.Grpc;

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

app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
    string.Join("\n", endpointSources.SelectMany(source => source.Endpoints.OfType<RouteEndpoint>().Select(e => e.RoutePattern.RawText))));

app.UseExceptionHandler();

app.MapControllers();
app.MapGrpcService<GrpcVRLearningSessionTeacherService>();

app.MapGet("/", () => "gRPC Server is running.");

await app.RunAsync();
