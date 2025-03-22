using GSOP.Domain.Contracts.Productions.Models;

namespace GSOP.Domain.Contracts.Productions;

/// <summary>
/// Manages production database logic
/// </summary>
public interface IProductionRepository
{
    /// <summary>
    /// Creates production in database
    /// </summary>
    /// <param name="production">Production</param>
    /// <returns>Generated id</returns>
    Task<long> CreateProduction(IProduction production);

    /// <summary>
    /// Deletes production
    /// </summary>
    /// <param name="id">Production id</param>
    /// <returns>Is production deleted</returns>
    Task<bool> DeleteProduction(ID id);

    /// <summary>
    /// Gets production by id
    /// </summary>
    /// <param name="id">Production id</param>
    /// <returns>Production data or null</returns>
    Task<ProductionDTO?> GetProduction(ID id);

    /// <summary>
    /// Is production already exists
    /// </summary>
    /// <param name="id">>Production id</param>
    /// <returns>True if production exists</returns>
    Task<bool> IsProductionExists(ID id);

    /// <summary>
    /// Gets production short information
    /// </summary>
    /// <returns>Each production info</returns>
    Task<IReadOnlyCollection<ProductionInfo>> GetProductionsInfo();

    /// <summary>
    /// Is production name already exists
    /// </summary>
    /// <param name="productionName">Production name</param>
    /// <returns>True if name is exists</returns>
    Task<bool> IsProductionNameExsits(ProductionName productionName);

    /// <summary>
    /// Updates production
    /// </summary>
    /// <param name="id">Production id</param>
    /// <param name="production">Production</param>
    Task UpdateProduction(ID id, IProduction production);
}
