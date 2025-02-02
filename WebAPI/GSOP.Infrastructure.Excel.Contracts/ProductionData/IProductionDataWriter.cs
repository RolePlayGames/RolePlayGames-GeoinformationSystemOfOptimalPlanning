namespace GSOP.Infrastructure.Excel.Contracts.ProductionData;

/// <summary>
/// Manages production data writing to excel logic
/// </summary>
public interface IProductionDataWriter
{
    /// <summary>
    /// Creates file stream wit exported to excel production data
    /// </summary>
    /// <param name="productionData">Production data</param>
    /// <returns></returns>
    Task<Stream> Write(Application.Contracts.ProductionData.ProductionData productionData);
}
