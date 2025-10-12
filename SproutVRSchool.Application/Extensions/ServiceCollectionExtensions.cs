using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Application.Behaviors;

namespace SproutVRSchool.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection service)
    {
        AddMediaR(service);

        return service;
    }

    /*
        Add MediatR for CQRS pattern
     */
    private static void AddMediaR(IServiceCollection service)
    {
        // Mediator
        service.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            // Add Pipeline Behaviors
            cfg.AddOpenBehaviors([
                    typeof(LoggingBehavior<,>),
                    typeof(ValidationBehavior<,>)
                ]);

        });

        // Validatiors of Fluent Validation
        service.AddValidatorsFromAssembly(typeof(ServiceCollectionServiceExtensions).Assembly);
    }
}
