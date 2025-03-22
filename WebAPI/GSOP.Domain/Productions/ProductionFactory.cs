using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Exceptions;
using GSOP.Domain.Contracts.Productions.Models;
using GSOP.Domain.Contracts.Locations;

namespace GSOP.Domain.Productions;

/// <inheritdoc/>
public class ProductionFactory : IProductionFactory
{
    private readonly IProductionRepository _productionRepository;

    public ProductionFactory(IProductionRepository productionRepository)
    {
        _productionRepository = productionRepository;
    }

    /// <inheritdoc/>
    public async Task<IProduction> CreateProduction(long id)
    {
        var productionId = new ID(id);

        var production = await _productionRepository.GetProduction(productionId) ?? throw new ProductionWasNotFoundException(productionId);

        var name = new ProductionName(production.Name);
        var coordinates = production.Coordinates is not null ? new Coordinates(new(production.Coordinates.Latitude), new(production.Coordinates.Longitude)) : null;

        return new Production(name, coordinates, _productionRepository);
    }

    /// <inheritdoc/>
    public async Task<IProduction> CreateProduction(ProductionDTO production)
    {
        var name = new ProductionName(production.Name);

        var isProductionNameExsits = await _productionRepository.IsProductionNameExsits(name);

        if (isProductionNameExsits)
            throw new ProductionNameAlreadyExistsException(name);

        var coordinates = production.Coordinates is not null ? new Coordinates(new(production.Coordinates.Latitude), new(production.Coordinates.Longitude)) : null;

        return new Production(name, coordinates,_productionRepository);
    }
}
