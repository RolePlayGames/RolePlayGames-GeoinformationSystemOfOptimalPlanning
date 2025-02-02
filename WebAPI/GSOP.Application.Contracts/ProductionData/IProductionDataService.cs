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
    /// <param name="shouldClearProductionData">Is production data should be cleared befor</param>
    Task Import(ProductionData data, bool shouldClearProductionData = false);

    /// <summary>
    /// Exports production data
    /// </summary>
    /// <returns>Production data</returns>
    Task<ProductionData> Export();
}
