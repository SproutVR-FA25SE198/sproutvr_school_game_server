using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.AIServices;

namespace SproutVRSchool.Infrastructure.Services.AIServices.Workers;

public class BigQuerySyncWorker : BackgroundService
{
    // =================================
    // === Fields
    // =================================

    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BigQuerySyncWorker> _logger;

    // =================================
    // === Constructors
    // =================================

    public BigQuerySyncWorker(
        ILogger<BigQuerySyncWorker> logger,
        IServiceProvider services,
        IConfiguration configuration)
    {
        _services = services;
        _logger = logger;
        _configuration = configuration;
    }


    // =================================
    // === Methods
    // =================================

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = _services.CreateScope())
            {
                IBigQuerySyncService syncService = scope.ServiceProvider.GetRequiredService<IBigQuerySyncService>();

                try
                {
                    await syncService.SyncAllTablesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sync BigQuery:{MessageError}", ex.Message);
                }
            }
            int minutes = _configuration.GetValue<int>("BigQuery:SyncTimeIntervalInMinutes");
            await Task.Delay(TimeSpan.FromMinutes(minutes), stoppingToken);
        }
    }
}
