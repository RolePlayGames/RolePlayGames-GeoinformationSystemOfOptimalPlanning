using GSOP.Domain.Contracts.ProductionLines.Models;

namespace GSOP.Application.Contracts.ProductionLines;

public interface IProductionLineService
{
    /// <summary>
    /// Creates new production line
    /// </summary>
    /// <param name="productionLine">Production line data</param>
    /// <returns>New production line ID</returns>
    Task<long> CreateProductionLine(ProductionLineDTO productionLine);

    /// <summary>
    /// Deletes production line
    /// </summary>
    /// <param name="id">Production line ID</param>
    Task DeleteProductionLine(long id);

    /// <summary>
    /// Gets production line by ID
    /// </summary>
    /// <param name="id">Production line ID</param>
    /// <returns>Production line</returns>
    Task<ProductionLineDTO> GetProductionLine(long id);

    /// <summary>
    /// Returns production lines small information
    /// </summary>
    Task<IReadOnlyCollection<ProductionLineInfo>> GetProductionLinesInfo();

    /// <summary>
    /// Updates production line
    /// </summary>
    /// <param name="id">Production line ID</param>
    /// <param name="productionLine">Production line data</param>
    Task UpdateProductionLine(long id, ProductionLineDTO productionLine);
}
