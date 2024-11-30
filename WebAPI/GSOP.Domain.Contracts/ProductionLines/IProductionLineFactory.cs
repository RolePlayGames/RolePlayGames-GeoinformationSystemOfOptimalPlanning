using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Domain.Contracts.ProductionLines
{
    /// <summary>
    /// Manages production line database logic
    /// </summary>
    public interface IProductionLineFactory
    {
        /// <summary>
        /// Creates production line by id from repository
        /// </summary>
        /// <param name="id">ProductionLine id</param>
        /// <returns>Production line</returns>
        Task<IProductionLine> Create(long id);

        /// <summary>
        /// Creates and validates production line by data
        /// </summary>
        /// <param name="productionLine">Production line data</param>
        /// <returns>Production line</returns>
        Task<IProductionLine> Create(ProductionLineDTO productionLine);
    }
}
