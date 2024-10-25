using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.Orders;
using GSOP.Domain.Contracts.ProductionLines;
using GSOP.Domain.Customers;
using GSOP.Domain.FilmRecipes;
using GSOP.Domain.FilmTypes;
using GSOP.Domain.Orders;
using GSOP.Domain.ProductionLines;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Domain.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add persistence for domain layer contracts
    /// </summary>
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddCustomerComponents()
            .AddFilmRecipeComponents()
            .AddFilmTypeComponents()
            .AddOrderComponents()
            .AddProductionLineComponents()
            ;

    internal static IServiceCollection AddCustomerComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<ICustomerFactory, CustomerFactory>();

    internal static IServiceCollection AddFilmRecipeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmRecipeFactory, FilmRecipeFactory>();

    internal static IServiceCollection AddFilmTypeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmTypeFactory, FilmTypeFactory>();

    internal static IServiceCollection AddOrderComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IOrderFactory, OrderFactory>();

    internal static IServiceCollection AddProductionLineComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionLineFactory, ProductionLineFactory>();
}
