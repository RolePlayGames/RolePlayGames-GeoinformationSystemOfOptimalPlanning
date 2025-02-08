namespace GSOP.Domain.Contracts.ProductionData;

/// <summary>
/// Manages production data import process start and end (by dispose)
/// </summary>
public interface IProductionDataImportProcess : IAsyncDisposable
{ 
    /// <summary>
    /// Deletes existing production data
    /// </summary>
    /// <returns></returns>
    Task DeleteProductionData();
}
