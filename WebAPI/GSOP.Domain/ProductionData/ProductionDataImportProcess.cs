using GSOP.Domain.Contracts.ProductionData;

namespace GSOP.Domain.ProductionData;

public class ProductionDataImportProcess : IProductionDataImportProcess
{
    private readonly IProductionDataRepository _repository;
    private readonly IAsyncDisposable _innerProcess;

    private bool _isDisposed = false;

    public ProductionDataImportProcess(IProductionDataRepository repository, IAsyncDisposable innerProcess)
    {
        _repository = repository;
        _innerProcess = innerProcess;
    }

    public Task DeleteProductionData()
    {
        return _repository.DeleteProductionData();
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
            return;

        await _repository.EndImport();
        await _innerProcess.DisposeAsync();

        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}
