namespace GSOP.Application.Contracts.ProductionData;

/// <summary>
/// Manages production data import export
/// </summary>
public interface IProductionDataService
{
    /// <summary>
    /// Imports production data
    /// </summary>
    /// <param name="data">Production data</param>
    Task Import(ProductionData data);

    /// <summary>
    /// Exports production data
    /// </summary>
    /// <returns>Production data</returns>
    Task<ProductionData> Export();
}
