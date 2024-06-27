using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Infrastructure.DataAccess.Connections;
using GSOP.Infrastructure.DataAccess.Contracts.Migrations;
using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.FilmRecipes;
using GSOP.Infrastructure.DataAccess.FilmTypes;
using GSOP.Infrastructure.DataAccess.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Infrastructure.DataAccess.DI;

/// <summary>
/// Contains Data Access DI extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers data access components
    /// </summary>
    public static IServiceCollection AddDataAccessComponents(this IServiceCollection serviceCollection)
        => serviceCollection
        .AddTransient<IConnectionStringProvider, ConnectionStringProvider>()
        .AddTransient<IMigrator, Migrator>()
        .AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped<IFilmRecipeRepository, FilmRecipeRepository>()
        .AddScoped<IFilmTypeRepository, FilmTypeRepository>()
        .AddMigratorConnection()
        .AddLinqToDbConnection();
}
