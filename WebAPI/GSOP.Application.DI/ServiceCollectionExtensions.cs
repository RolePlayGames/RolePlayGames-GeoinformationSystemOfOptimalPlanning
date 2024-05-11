using GSOP.Application.Contracts.Customers;
using GSOP.Application.Contracts.FilmTypes;
using GSOP.Application.Customers;
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
            .AddFilmTypeComponents();

    internal static IServiceCollection AddCustomerComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<ICustomerService, CustomerService>();

    internal static IServiceCollection AddFilmTypeComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IFilmTypeSerivce, FilmTypeService>();
}
