using GSOP.Application.Contracts.Productions;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Exceptions;
using GSOP.Domain.Contracts.Productions.Models;

namespace GSOP.Application.Productions;

/// <inheritdoc/>
public class ProductionService : IProductionService
{
    private readonly IProductionFactory _productionFactory;
    private readonly IProductionRepository _productionRepository;

    public ProductionService(IProductionFactory productionFactory, IProductionRepository productionRepository)
    {
        _productionFactory = productionFactory;
        _productionRepository = productionRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateProduction(ProductionDTO production)
    {
        var newProduction = await _productionFactory.CreateProduction(production);

        return await _productionRepository.CreateProduction(newProduction);
    }

    /// <inheritdoc/>
    public async Task DeleteProduction(long id)
    {
        var productionId = new ID(id);

        var isProductionDeleted = await _productionRepository.DeleteProduction(productionId);

        if (!isProductionDeleted)
            throw new ProductionWasNotFoundException(productionId);
    }

    /// <inheritdoc/>
    public async Task<ProductionDTO> GetProduction(long id)
    {
        var productionId = new ID(id);
        var production = await _productionRepository.GetProduction(productionId);

        return production is null ? throw new ProductionWasNotFoundException(productionId) : production;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<ProductionInfo>> GetProductionsInfo()
    {
        return _productionRepository.GetProductionsInfo();
    }

    /// <inheritdoc/>
    public async Task UpdateProduction(long id, ProductionDTO production)
    {
        var productionId = new ID(id);
        var productionName = new ProductionName(production.Name);

        var existingProduction = await _productionFactory.CreateProduction(id);

        await existingProduction.SetName(productionName);
        existingProduction.SetCoordinates(production.Coordinates is null ? null : new(new(production.Coordinates.Latitude), new(production.Coordinates.Longitude)));

        await _productionRepository.UpdateProduction(productionId, existingProduction);
    }
}
