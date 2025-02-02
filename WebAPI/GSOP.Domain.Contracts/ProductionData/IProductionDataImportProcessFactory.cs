namespace GSOP.Domain.Contracts.ProductionData;

/// <summary>
/// Manages creating production data import process
/// </summary>
public interface IProductionDataImportProcessFactory
{
    /// <summary>
    /// Creates production data import process
    /// </summary>
    /// <returns>Disposible import process</returns>
    Task<IProductionDataImportProcess> Create();
}
