using GSOP.Application.Contracts.Customers;
using GSOP.Application.Contracts.FilmRecipes;
using GSOP.Application.Contracts.FilmTypes;
using GSOP.Application.Customers;
using GSOP.Application.FilmRecipes;
using GSOP.Application.FilmTypes;
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
            .AddFilmTypeComponents();

    internal static IServiceCollection AddCustomerComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<ICustomerService, CustomerService>();

    internal static IServiceCollection AddFilmRecipeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmRecipeService, FilmRecipeService>();

    internal static IServiceCollection AddFilmTypeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmTypeSerivce, FilmTypeService>();
}
