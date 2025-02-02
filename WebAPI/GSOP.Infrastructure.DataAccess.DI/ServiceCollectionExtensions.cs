using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionData;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Infrastructure.DataAccess.Connections;
using GSOP.Infrastructure.DataAccess.Contracts.Migrations;
using GSOP.Infrastructure.DataAccess.Customers;
using GSOP.Infrastructure.DataAccess.FilmRecipes;
using GSOP.Infrastructure.DataAccess.FilmTypes;
using GSOP.Infrastructure.DataAccess.Migrations;
using GSOP.Infrastructure.DataAccess.Orders;
using GSOP.Infrastructure.DataAccess.ProductionData;
using GSOP.Infrastructure.DataAccess.ProductionLines;
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
        .AddScoped<IOrderRepository, OrderRepository>()
        .AddScoped<IProductionLineRepository, ProductionLineRepository>()
        .AddScoped<IProductionDataRepository, ProductionDataRepository>()
        .AddMigratorConnection()
        .AddLinqToDbConnection();
}
