using HackerNews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeedWork.MediatR;

namespace SantanderAscendion.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.AddSeedWork(assemblies);
        services.AddHackerNewsClient(configuration);
    }
}
