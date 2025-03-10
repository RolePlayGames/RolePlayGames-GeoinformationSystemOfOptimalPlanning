using GSOP.Application.Contracts.Customers;
using GSOP.Application.Contracts.FilmRecipes;
using GSOP.Application.Contracts.FilmTypes;
using GSOP.Application.Contracts.Orders;
using GSOP.Application.Contracts.ProductionData;
using GSOP.Application.Contracts.ProductionLines;
using GSOP.Application.Customers;
using GSOP.Application.FilmRecipes;
using GSOP.Application.FilmTypes;
using GSOP.Application.Optimization;
using GSOP.Application.Orders;
using GSOP.Application.ProductionData;
using GSOP.Application.ProductionLines;
using GSOP.Domain.Contracts.Optimization;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Application.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add persistence for application layer contracts
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddCustomerComponents()
            .AddFilmRecipeComponents()
            .AddFilmTypeComponents()
            .AddOrderComponents()
            .AddProductionLineComponents()
            .AddProductionDataComponents()
            .AddOptimization()
            ;

    internal static IServiceCollection AddCustomerComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<ICustomerService, CustomerService>();

    internal static IServiceCollection AddFilmRecipeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmRecipeService, FilmRecipeService>();

    internal static IServiceCollection AddFilmTypeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmTypeSerivce, FilmTypeService>();

    internal static IServiceCollection AddOrderComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IOrderService, OrderService>();

    internal static IServiceCollection AddProductionLineComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionLineService, ProductionLineService>();

    internal static IServiceCollection AddProductionDataComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionDataService, ProductionDataService>();

    internal static IServiceCollection AddOptimization(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IProductionPlanner, ProductionPlanner>();
}
