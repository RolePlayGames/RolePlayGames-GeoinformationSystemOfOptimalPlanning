using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Domain.Contracts.Routes;

/// <summary>
/// Manages route domain model creation
/// </summary>
public interface IRouteFactory
{
    /// <summary>
    /// Creates route by id from repository
    /// </summary>
    /// <param name="id">Route id</param>
    /// <returns>Route</returns>
    Task<IRoute> Create(long id);

    /// <summary>
    /// Creates routes for production
    /// </summary>
    /// <param name="ProductionId">Production id</param>
    /// <returns>New routes</returns>
    Task<IReadOnlyCollection<IRoute>> CreateProductionRoutes(ID ProductionId);

    /// <summary>
    /// Creates routes for customer
    /// </summary>
    /// <param name="CustomerId">Customer id</param>
    /// <returns>New routes</returns>
    Task<IReadOnlyCollection<IRoute>> CreateCustomerRoutes(ID CustomerId);
}
