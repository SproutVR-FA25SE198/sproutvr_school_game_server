using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using SproutVRSchool.Application.Abstractions.AccountServices;
using SproutVRSchool.Application.Abstractions.Clock;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Abstractions.RoomServices.CodeGenerator;
using SproutVRSchool.Application.Abstractions.RoomServices.SessionValidator;
using SproutVRSchool.Application.Abstractions.RoomServices.TeacherSession;
using SproutVRSchool.Application.Abstractions.RoomServices.VRGlassSession;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Infrastructure.Data;
using SproutVRSchool.Infrastructure.Data.Seeders;
using SproutVRSchool.Infrastructure.Repositories;
using SproutVRSchool.Infrastructure.Services.AccountServices;
using SproutVRSchool.Infrastructure.Services.BackgroundServices;
using SproutVRSchool.Infrastructure.Services.Clock;
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
        service.AddFileHelpers(configuration);

        service.AddDbContextAndSeeders(configuration);

        service.AddRepositories();

        service.AddProviders();

        service.AddRedisStack(configuration);

        service.AddVRLearningSessionService();

        service.AddBackgroundService();

        service.AddAccountsService();

        service.AddRetryPolicyRegistry();

        return service;
    }

    /*
        Contains services related to user accounts includning OTP, JWT Tokens, etc.
     */
    private static void AddAccountsService(
        this IServiceCollection service)
    {
        service.AddScoped<ITokenService, JwtTokenService>();
    }

    /*
        Add Retry Policy Registry using Polly
     */
    private static void AddRetryPolicyRegistry(this IServiceCollection service)
    {
        // Configure retry policyes for Redis transaction
        service.AddResiliencePipeline<string, bool>(
            AppCts.RetryKeys.REDIS_TRANSACTION_KEY,
            (pipelineBuilder) =>
        {
            pipelineBuilder.AddRetry(new RetryStrategyOptions<bool>
            {
                ShouldHandle = new PredicateBuilder<bool>().HandleResult(false),

                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential, // Wait 50ms, then 100ms, then 200ms
                Delay = TimeSpan.FromMilliseconds(50)
            });
        });
    }

    /*
        Configure for DbContext & Seedings
     */
    private static void AddDbContextAndSeeders(
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
        service.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    /// <summary>
    /// Related to the core domain business logic
    /// </summary>
    /// <param name="service"></param>
    private static void AddVRLearningSessionService(this IServiceCollection service)
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

    /// <summary>
    /// Using all file helpers
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    private static void AddFileHelpers(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddTransient<IFileReader, JsonFileReader>();

        string? contentRootPath = configuration.GetValue<string>("FileLocalStorageSettings:ContentRootPath");

        service.AddSingleton<LocalStorageService>(sp =>
            new LocalStorageService(contentRootPath!));

        service.AddSingleton<ILocalStorageService>(sp => sp.GetRequiredService<LocalStorageService>());
        service.AddSingleton<IPathService>(sp => sp.GetRequiredService<LocalStorageService>());
    }

    /// <summary>
    /// Add group of backgrounds to handle computational parts
    /// </summary>
    /// <param name="service"></param>
    private static void AddBackgroundService(this IServiceCollection service)
    {
        service.AddHostedService<ConsumerTaskUpdateBackgroundService>();
    }
}
