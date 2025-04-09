using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Application.Contracts.Routes;

/// <summary>
/// Manages route use cases
/// </summary>
public interface IRouteService
{
    /// <summary>
    /// Gets route by ID
    /// </summary>
    /// <param name="id">Route ID</param>
    /// <returns>Route</returns>
    Task<RouteReadDTO> GetRoute(long id);

    /// <summary>
    /// Returns routes small information
    /// </summary>
    Task<IReadOnlyCollection<RouteInfo>> GetRoutesInfo();

    /// <summary>
    /// Updates route
    /// </summary>
    /// <param name="id">Route ID</param>
    /// <param name="route">Route data</param>
    Task UpdateRoute(long id, RouteWriteDTO route);
}
