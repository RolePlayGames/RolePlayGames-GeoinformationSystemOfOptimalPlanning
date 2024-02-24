using GSOP.Infrastructure.DataAccess.Connections;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Infrastructure.DataAccess;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers data access components
    /// </summary>
    public static IServiceCollection AddLinqToDbConnection(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddLinqToDBContext<DatabaseConnection>((provider, options) => 
        {
            var connectionProvider = provider.CreateAsyncScope().ServiceProvider.GetService<IConnectionStringProvider>()
                ?? throw new InvalidOperationException();

            return options
                .UsePostgreSQL(connectionProvider.DmlConnectionString)
                .UseDefaultLogging(provider);
        });
}
