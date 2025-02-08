using GSOP.Domain.Contracts.ProductionData;

namespace GSOP.Domain.ProductionData;

public class ProductionDataImportProcessFactory : IProductionDataImportProcessFactory
{
    private readonly IProductionDataRepository _productionDataRepository;

    public ProductionDataImportProcessFactory(IProductionDataRepository productionDataRepository)
    {
        _productionDataRepository = productionDataRepository;
    }

    public async Task<IProductionDataImportProcess> Create()
    {
        var process = await _productionDataRepository.StartImport();

        return new ProductionDataImportProcess(_productionDataRepository, process);
    }
}
