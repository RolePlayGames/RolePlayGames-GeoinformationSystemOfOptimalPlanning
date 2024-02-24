using GSOP.Domain.Contracts.Customers;
using GSOP.Domain.Customers;
using Microsoft.Extensions.DependencyInjection;

namespace GSOP.Domain.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add persistence for domain layer contracts
    /// </summary>
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddCustomerComponents();

    internal static IServiceCollection AddCustomerComponents(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<ICustomerFactory, CustomerFactory>();
}
