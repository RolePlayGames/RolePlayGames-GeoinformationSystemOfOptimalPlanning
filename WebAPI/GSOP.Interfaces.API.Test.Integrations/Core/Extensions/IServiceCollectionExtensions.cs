using GSOP.Interfaces.API.Test.Integrations.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Interfaces.API.Test.Integrations.Core.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds web helper
        /// </summary>
        /// <param name="webHelper">Web helper</param>
        public static IServiceCollection AddWebHelper(this IServiceCollection services, IWebHelper webHelper)
        {
            webHelper.AddServices(services);
            return services;
        }

        /// <summary>
        /// Adds core helper
        /// </summary>
        public static IServiceCollection AddCoreHelper(this IServiceCollection services)
        {
            return services.AddWebHelper(new CoreHelper());
        }
    }
}
