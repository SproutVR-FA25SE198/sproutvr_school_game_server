using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Infrastructure.Database;

namespace SproutVRSchool.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    /*
        Configure for DbContext
     */
    public static IServiceCollection AddSchoolServerDbContext(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddDbContext<SchoolServerDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        return service;
    }
}
