using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Infrastructure.Data;

namespace SproutVRSchool.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddPersistence(configuration);
        return service;
    }

    /*
        Configure for DbContext
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

        service.AddTransient<IFileReader, FileReader>();

        service.AddTransient<IDataSeeder, JsonDataSeeder<SchoolServerDbContext>>();
    }
}
