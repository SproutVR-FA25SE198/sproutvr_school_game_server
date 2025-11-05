using Asp.Versioning;
using SproutVRSchool.Presentation.ExceptionHandlers;
using SproutVRSchool.Presentation.Interceptors.UnaryUnary;

namespace SproutVRSchool.Presentation.Extensions;

internal static partial class ServiceCollectionExtensions
{
    // =========================================
    // === Entry Point for service collections
    // =========================================

    public static IServiceCollection AddPresentation(
        this IServiceCollection service, IConfiguration configuration)
    {

        service.AddControllersConfigs();

        service.AddApiVersioning();

        service.AddExceptionHandlers();

        service.AddGrpcConfigs();

        service.AddCorsConfigs(configuration);

        return service;
    }

    // =========================================
    // === Services
    // =========================================

    /*
        Add CORS for desktop app clients
     */
    private static void AddCorsConfigs(
        this IServiceCollection service, IConfiguration configuration)
    {
        string? desktopAppUrl = configuration.GetValue<string>("DesktopAppUrl");
        service.AddCors(options =>
        {
            options.AddPolicy("DesktopAppPolicy", policy =>
            {
                if (!string.IsNullOrEmpty(desktopAppUrl))
                {
                    // Configure specific origin for the desktop app, allowing headers and credentials
                    // Note: AllowCredentials requires specifying the origin(s), not using AllowAnyOrigin()
                    policy.WithOrigins(desktopAppUrl)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                }
                else
                {
                    // Fallback for maximum flexibility (e.g., if DesktopAppUrl is missing, or for general development)
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                }
            });
        });
    }

    /*
        Add configuration for controllers to use the fluent validations instead of default model state 
     */
    private static void AddControllersConfigs(this IServiceCollection service)
    {
        service.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
    }

    /*
        Api Versioning
     */
    private static void AddApiVersioning(
        this IServiceCollection service)
    {
        service.AddApiVersioning(opt =>
        {
            // major, minor
            opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);

            opt.AssumeDefaultVersionWhenUnspecified = true;

            opt.ReportApiVersions = true;

            // Reads the version number from the URL segment (e.g. .../api/v1/devices/...)
            // Reads the version number from a query string parameter (e.g. .../api/devices?api-version=1.0)
            // Reads the version number from a header (e.g. X-Version: 1.0)
            // Reads the version number from the media type (e.g. application/json;v=1.0)
            opt.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("X-Version"),
                    new UrlSegmentApiVersionReader());
        }).AddApiExplorer(opt =>
        {
            opt.SubstituteApiVersionInUrl = true;
        });

        // suport for versioning in swagger
        service.AddEndpointsApiExplorer();
    }

    /*
        GrpcConfiguration 
     */
    private static void AddGrpcConfigs(
        this IServiceCollection service)
    {
        service.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<NotFoundInterceptor>();
        });

        service.AddGrpcReflection();
    }

    /*
        Configure Exception Handlers Pipeline
     */
    private static void AddExceptionHandlers(
        this IServiceCollection service)
    {
        service.AddProblemDetails(cfg =>
        {
            cfg.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
            };
        });

        service.AddExceptionHandler<AccountExceptionHandler>();
        service.AddExceptionHandler<ResourcesExceptionHandler>();
        service.AddExceptionHandler<ContentSeedingsExceptionHandler>();
        service.AddExceptionHandler<GeneralExceptionHandler>();
    }
}
