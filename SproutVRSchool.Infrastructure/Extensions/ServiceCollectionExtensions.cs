using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Infrastructure.FileHelpers;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Infrastructure.Clock;
using SproutVRSchool.Infrastructure.Data;
using SproutVRSchool.Infrastructure.Data.Seeders;
using SproutVRSchool.Infrastructure.Repositories;
using StackExchange.Redis;
using SproutVRSchool.Application.Abstractions.FileHelpers;
using SproutVRSchool.Application.Abstractions.Services.CodeGenerator;
using SproutVRSchool.Application.Abstractions.Services.SessionValidator;
using SproutVRSchool.Application.Abstractions.Services.VRGlassSession;
using SproutVRSchool.Infrastructure.Services;

namespace SproutVRSchool.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddPersistence(configuration);

        service.AddRepositories();

        service.AddProviders();

        service.AddRedisStack(configuration);

        service.AddBusinessServices();

        return service;
    }

    /*
        Configure for DbContext & Seedings
     */
    private static void AddPersistence(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddDbContext<SchoolServerDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        service.AddScoped<SchoolServerDbContextSeeder>();

        service.AddTransient<IFileReader, JsonFileReader>();

        service.AddScoped<ISchoolServerDbContext>(provider => provider.GetRequiredService<SchoolServerDbContext>());

        service.AddTransient<IDataSeeder, JsonDataSeeder<SchoolServerDbContext>>();
    }

    private static void AddRepositories(this IServiceCollection service)
    {
        service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        service.AddScoped<IUnitOfWork, UnitOfWork<SchoolServerDbContext>>();
    }

    private static void AddProviders(this IServiceCollection service)
    {
        service.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }

    private static void AddBusinessServices(this IServiceCollection service)
    {
        service.AddSingleton<ICodeGeneratorService, CodeGenerator>();
    }

    /*
        Using Redis Stream and Redis Modules
     */
    private static void AddRedisStack(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        string redisStackConnection = configuration.GetConnectionString("RedisStack");
        service.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisStackConnection!));
    }
}
