namespace GSOP.Domain.Contracts.ProductionData;

/// <summary>
/// Manages common work with production data
/// </summary>
public interface IProductionDataRepository
{
    /// <summary>
    /// Deletes existing production data
    /// </summary>
    /// <returns></returns>
    Task DeleteProductionData();

    /// <summary>
    /// Starts import process in DB
    /// </summary>
    /// <returns>Disposable import process</returns>
    Task<IAsyncDisposable> StartImport();

    /// <summary>
    /// Ends import process
    /// </summary>
    Task EndImport();
}
