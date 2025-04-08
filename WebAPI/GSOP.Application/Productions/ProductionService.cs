using GSOP.Application.Contracts.Productions;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Exceptions;
using GSOP.Domain.Contracts.Productions.Models;
using GSOP.Domain.Contracts.Routes;

namespace GSOP.Application.Productions;

/// <inheritdoc/>
public class ProductionService : IProductionService
{
    private readonly IProductionFactory _productionFactory;
    private readonly IProductionRepository _productionRepository;
    private readonly IRouteFactory _routeFactory;
    private readonly IRouteRepository _routeRepository;

    public ProductionService(IProductionFactory productionFactory, IProductionRepository productionRepository, IRouteFactory routeFactory, IRouteRepository routeRepository)
    {
        _productionFactory = productionFactory;
        _productionRepository = productionRepository;
        _routeFactory = routeFactory;
        _routeRepository = routeRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateProduction(ProductionDTO production)
    {
        var newProduction = await _productionFactory.CreateProduction(production);

        var productionId = await _productionRepository.CreateProduction(newProduction);

        var routes = await _routeFactory.CreateProductionRoutes(new(productionId));

        foreach (var route in routes)
        {
            await _routeRepository.Create(route);
        }

        return productionId;
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
