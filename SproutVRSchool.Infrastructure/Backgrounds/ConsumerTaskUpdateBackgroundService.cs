using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession.Dtos;
using SproutVRSchool.Domain;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Backgrounds;

public class ConsumerTaskUpdateBackgroundService : BackgroundService
{
    private readonly ILogger<ConsumerTaskUpdateBackgroundService> _logger;
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonOptions;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public ConsumerTaskUpdateBackgroundService(ILogger<ConsumerTaskUpdateBackgroundService> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _database = redis.GetDatabase();
        _jsonOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConsumerTaskUpdateBackgroundService is starting.");

        // 1. Active Room discovery to scan all active VR lessions
        // - Each vr learning session is assigned to 1 worker thread for consuming task update

        return Task.CompletedTask;
    }
}
