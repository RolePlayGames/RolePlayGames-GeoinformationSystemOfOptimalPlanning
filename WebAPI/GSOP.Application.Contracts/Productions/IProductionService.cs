using GSOP.Domain.Contracts.Productions.Models;

namespace GSOP.Application.Contracts.Productions;

/// <summary>
/// Manages production use cases
/// </summary>
public interface IProductionService
{
    /// <summary>
    /// Creates new production
    /// </summary>
    /// <param name="production">Production data</param>
    /// <returns>New production ID</returns>
    Task<long> CreateProduction(ProductionDTO production);

    /// <summary>
    /// Deletes production
    /// </summary>
    /// <param name="id">Production ID</param>
    Task DeleteProduction(long id);

    /// <summary>
    /// Gets production by ID
    /// </summary>
    /// <param name="id">Production ID</param>
    /// <returns>Production</returns>
    Task<ProductionDTO> GetProduction(long id);

    /// <summary>
    /// Returns productions small information
    /// </summary>
    Task<IReadOnlyCollection<ProductionInfo>> GetProductionsInfo();

    /// <summary>
    /// Updates production
    /// </summary>
    /// <param name="id">Production ID</param>
    /// <param name="production">Production data</param>
    Task UpdateProduction(long id, ProductionDTO production);
}
