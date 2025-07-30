using Microsoft.Extensions.DependencyInjection;
using SeedWork.MediatR.Behaviors;
using System.Reflection;

namespace SeedWork.MediatR;

public static class ServiceCollectionExtensions
{
    public static void AddSeedWork(this IServiceCollection services,
        Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        services.AddDistributedMemoryCache();
    }
}
