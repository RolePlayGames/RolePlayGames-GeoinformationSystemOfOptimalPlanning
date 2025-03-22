using GSOP.Domain.Contracts.Productions.Models;

namespace GSOP.Domain.Contracts.Productions;

/// <summary>
/// Manages production domain model creation
/// </summary>
public interface IProductionFactory
{
    /// <summary>
    /// Creates production by id from repository
    /// </summary>
    /// <param name="id">Production id</param>
    /// <returns>Production</returns>
    Task<IProduction> CreateProduction(long id);

    /// <summary>
    /// Creates and validates production by data
    /// </summary>
    /// <param name="production">Production data</param>
    /// <returns>Production</returns>
    Task<IProduction> CreateProduction(ProductionDTO production);
}
