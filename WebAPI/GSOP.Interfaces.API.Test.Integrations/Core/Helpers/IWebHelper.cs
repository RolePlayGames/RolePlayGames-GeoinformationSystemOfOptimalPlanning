using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.Core.Helpers;

/// <summary>
/// Represents web helper that can be built into factory
/// </summary>
internal interface IWebHelper
{
    /// <summary>
    /// Adds helper's services to builder
    /// </summary>
    /// <param name="serviceCollection">Builder service collection</param>
    void AddServices(IServiceCollection serviceCollection);
}
