using GSOP.Interfaces.API.Home.PathGetters;

namespace GSOP.Interfaces.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers web API components
        /// </summary>
        internal static IServiceCollection AddWebApiComponents(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddSingleton<IPathGetter, PathGetter>()
                .AddStaticClassesProxies();
    }
}
