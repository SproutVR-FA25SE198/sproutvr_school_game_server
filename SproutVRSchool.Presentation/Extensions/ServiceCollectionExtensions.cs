using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Domain.Abstractions;
using SproutVRSchool.Infrastructure.Data;
using SproutVRSchool.Infrastructure.Data.Seeders;
using SproutVRSchool.Infrastructure.FileServices;
using SproutVRSchool.Infrastructure.Repositories;

namespace SproutVRSchool.Presentation.Extensions;

internal static partial class ServiceCollectionExtensions
{
    // =========================================
    // === Entry Point for service collections
    // =========================================
    public static IServiceCollection AddPresentation(
        this IServiceCollection service,
        IConfiguration configuration)
    {

        service.AddApiVersioning();

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
        });

        // suport for versioning in swagger
        service.AddEndpointsApiExplorer();
    }

}
