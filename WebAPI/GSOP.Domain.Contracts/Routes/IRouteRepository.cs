using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Domain.Contracts.Routes;

/// <summary>
/// Manages route database logic
/// </summary>
public interface IRouteRepository
{
    /// <summary>
    /// Creates route in database
    /// </summary>
    /// <param name="route">Route</param>
    /// <returns>Generated id</returns>
    Task<long> Create(IRoute route);

    /// <summary>
    /// Deletes route
    /// </summary>
    /// <param name="id">Route id</param>
    /// <returns>Is route deleted</returns>
    Task<bool> Delete(ID id);

    /// <summary>
    /// Gets route by id
    /// </summary>
    /// <param name="id">Route id</param>
    /// <returns>Route data or null</returns>
    Task<RouteReadDTO?> Get(ID id);

    /// <summary>
    /// Gets route short information
    /// </summary>
    /// <returns>Each route info</returns>
    Task<IReadOnlyCollection<RouteInfo>> GetInfos();

    /// <summary>
    /// Updates route
    /// </summary>
    /// <param name="id">Route id</param>
    /// <param name="route">Route</param>
    Task Update(ID id, IRoute route);

    /// <summary>
    /// Gets routes between productions and customers
    /// </summary>
    /// <param name="productionIds">Productions</param>
    /// <param name="customerIds">Customers</param>
    /// <returns>Sutable routes</returns>
    Task<IReadOnlyCollection<RouteReadDTO>> GetRoutesBetweenProductionsAndCustomers(IReadOnlyCollection<ID> productionIds, IReadOnlyCollection<ID> customerIds);
}
