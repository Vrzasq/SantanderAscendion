using HackerNews.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackerNews;

public static class ServiceCollectionExtensions
{
    public static void AddHackerNewsClient(this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var hackerNewsConfig = configuration.GetSection(HackerNewsConfig.SectionName);
        services.Configure<HackerNewsConfig>(hackerNewsConfig);

        services.AddHttpClient<HackerNewsClient>((sp, configure) =>
        {
            var options = sp.GetRequiredService<IOptions<HackerNewsConfig>>().Value;
            configure.BaseAddress = new Uri(options.BaseUrl);
            configure.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        })
        .AddStandardResilienceHandler();

        services.AddScoped<IHackerNewsClient, HackerNewsCachedClient>();

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        services.AddKeyedSingleton(HackerNewsSerializationOptions.Name, HackerNewsSerializationOptions.Default);
    }
}
