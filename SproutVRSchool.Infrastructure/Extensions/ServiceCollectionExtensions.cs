using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Infrastructure.Backgrounds;
using SproutVRSchool.Infrastructure.Clock;
using SproutVRSchool.Infrastructure.Data;
using SproutVRSchool.Infrastructure.Data.Seeders;
using SproutVRSchool.Infrastructure.Repositories;
using SproutVRSchool.Infrastructure.Services.FileServices;
using SproutVRSchool.Infrastructure.Services.RoomServices;
using StackExchange.Redis;

namespace SproutVRSchool.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddFileHelpers();

        service.AddPersistence(configuration);

        service.AddRepositories();

        service.AddProviders();

        service.AddRedisStack(configuration);

        service.AddBusinessServices();

        service.AddBackgrounds();

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

        service.AddIdentity<UserAccount, UserAccountRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<SchoolServerDbContext>();

        service.AddScoped<IdentityDbContextSeeder>();

        service.AddScoped<SchoolServerDbContextSeeder>();

        service.AddScoped<ISchoolServerDbContext>(provider => provider.GetRequiredService<SchoolServerDbContext>());

        service.AddTransient<IDataSeeder, JsonDataSeeder<SchoolServerDbContext>>();
    }

    /// <summary>
    /// Related to the application's repositories
    /// </summary>
    /// <param name="service"></param>

    private static void AddRepositories(this IServiceCollection service)
    {
        service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        service.AddScoped<IUnitOfWork, UnitOfWork<SchoolServerDbContext>>();
    }

    /// <summary>
    /// Related to the external providers
    /// </summary>
    /// <param name="service"></param>
    private static void AddProviders(this IServiceCollection service)
    {
        service.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }

    /// <summary>
    /// Related to the core domain business logic
    /// </summary>
    /// <param name="service"></param>
    private static void AddBusinessServices(this IServiceCollection service)
    {
        service.AddScoped<IVRLearningSessionTeacherService, RedisTeacherVRLearningSessionService>();
        service.AddScoped<IVRLearningSessionWithVRGlassService, RedisVRGlassVRLearningSessionService>();
        service.AddScoped<IVRLearningSessionValidator, RoomValidator>();
        service.AddSingleton<ICodeGeneratorService, CodeGenerator>();
    }

    /// <summary>
    /// Using Redis Stream and Redis Modules
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    private static void AddRedisStack(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        string redisStackConnection = configuration.GetConnectionString("RedisStack");
        service.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisStackConnection!));
    }

    /*
        Using all file helpers
     */
    private static void AddFileHelpers(this IServiceCollection service)
    {
        service.AddTransient<IFileReader, JsonFileReader>();
        service.AddTransient<ILocalStorageService, LocalStorageService>();
        service.AddTransient<IPathService, LocalStorageService>();
    }

    private static void AddBackgrounds(this IServiceCollection service)
    {
        service.AddHostedService<ConsumerTaskUpdateBackgroundService>();
    }
}
