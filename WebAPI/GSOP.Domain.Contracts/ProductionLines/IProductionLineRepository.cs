using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines
{
    /// <summary>
    /// Manages production line database logic
    /// </summary>
    public interface IProductionLineRepository
    {
        /// <summary>
        /// Creates production line in database
        /// </summary>
        /// <param name="productionLine">Order</param>
        /// <returns>Generated id</returns>
        Task<long> Create(IProductionLine productionLine);

        /// <summary>
        /// Deletes production line
        /// </summary>
        /// <param name="id">Production line id</param>
        /// <returns>Is production line deleted</returns>
        Task<bool> Delete(ID id);

        /// <summary>
        /// Gets production line by id
        /// </summary>
        /// <param name="id">Production line id</param>
        /// <returns>Production line data or null</returns>
        Task<ProductionLineDTO?> Get(ID id);

        /// <summary>
        /// Gets production lines short information
        /// </summary>
        /// <returns>Each production line info</returns>
        Task<IReadOnlyCollection<ProductionLineInfo>> GetInfos();

        /// <summary>
        /// Is production line number already exists
        /// </summary>
        /// <param name="name">Production line name</param>
        /// <returns>True if order number exists</returns>
        Task<bool> IsNameExists(ProductionLineName name);

        /// <summary>
        /// Updates production line
        /// </summary>
        /// <param name="id">Production line id</param>
        /// <param name="productionLine">Production line</param>
        Task Update(ID id, IProductionLine productionLine);
    }
}
