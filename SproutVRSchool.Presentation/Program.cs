WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ======================================
// Add services to the container.
// ======================================

builder.Services.AddControllers();

WebApplication app = builder.Build();

// ======================================
// Configure the HTTP request pipeline.
// ======================================

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);
