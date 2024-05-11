using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.Core;

public abstract class WebIntegrationTestBase
{
    protected static Fixture Fixture { get; } = new();

    private HttpClient? _httpClient;

    protected HttpClient Client => _httpClient ??= CreateClient();

    private protected WebApplicationFactory<Program> Factory { get; }

    protected WebIntegrationTestBase()
    {
        Factory = new WebApplicationFactory<Program>();
    }

    protected HttpClient CreateClient()
    {
        return Factory
            .WithWebHostBuilder(builder =>
                builder.ConfigureServices(services => ConfigureServices(services)))
            .CreateClient();
    }

    /// <summary>
    /// Configures host services
    /// </summary>
    /// <param name="services">Service collection</param>
    protected virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services;
    }
}
