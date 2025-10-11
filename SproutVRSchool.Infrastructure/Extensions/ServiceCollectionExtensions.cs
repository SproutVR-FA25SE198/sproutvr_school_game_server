using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace SproutVRSchool.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddPersistence(configuration);

        service.AddRepositories();

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
}
