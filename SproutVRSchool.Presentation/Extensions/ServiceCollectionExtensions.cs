using Asp.Versioning;
using SproutVRSchool.Presentation.ExceptionHandlers;

namespace SproutVRSchool.Presentation.Extensions;

internal static partial class ServiceCollectionExtensions
{
    // =========================================
    // === Entry Point for service collections
    // =========================================
    public static IServiceCollection AddPresentation(
        this IServiceCollection service)
    {

        service.AddApiVersioning();
        service.AddExceptionHandlers();

        return service;
    }

    // =========================================
    // === Services
    // =========================================

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

        service.AddExceptionHandler<ValidationExceptionHandler>();
        service.AddExceptionHandler<NotFoundExceptionHandler>();
        service.AddExceptionHandler<GeneralExceptionHandler>();
    }
}
