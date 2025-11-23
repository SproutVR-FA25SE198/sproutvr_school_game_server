using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SproutVRSchool.Infrastructure.Services.AIServices;
public class BigQueryWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public BigQueryWorker(IServiceProvider services, IConfiguration configuration)
    {
        _services = services;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = _services.CreateScope())
            {
                BigQuerySyncService syncService = scope.ServiceProvider.GetRequiredService<BigQuerySyncService>();

                try
                {
                    await syncService.SyncAllTablesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi Sync BigQuery: {ex.Message}");
                }
            }
            int minutes = _configuration.GetValue<int>("BigQuery:SyncTimeIntervalInMinutes");
            await Task.Delay(TimeSpan.FromMinutes(minutes), stoppingToken);
        }
    }
}
