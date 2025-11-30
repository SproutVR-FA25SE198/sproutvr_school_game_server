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

        service.AddHttpClient();

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
            cfg.RegisterServicesFromAssemblies(typeof(ServiceCollectionExtensions).Assembly);

            // Add Pipeline Behaviors
            cfg.AddOpenBehaviors([
                    typeof(LoggingBehavior<,>),
                    typeof(ValidationBehavior<,>)
                ]);
        });

        // Validatiors of Fluent Validation
        service.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
    }
}
