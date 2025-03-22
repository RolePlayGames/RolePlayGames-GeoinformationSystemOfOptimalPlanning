using GSOP.Domain.Contracts.Productions;
using GSOP.Domain.Contracts.Productions.Exceptions;
using GSOP.Domain.Contracts.Productions.Models;
using GSOP.Domain.Contracts.Locations;

namespace GSOP.Domain.Productions;

/// <inheritdoc/>
public class Production : IProduction
{
    private readonly IProductionRepository _productionRepository;

    public ProductionName Name { get; protected set; }

    public Coordinates? Coordinates { get; protected set; }

    public Production(ProductionName name, Coordinates? coordinates, IProductionRepository productionRepository)
    {
        Name = name;
        _productionRepository = productionRepository;
        Coordinates = coordinates;
    }

    public async Task SetName(ProductionName name)
    {
        if (Name != name)
        {
            var isNameExists = await _productionRepository.IsProductionNameExsits(name);

            if (isNameExists)
                throw new ProductionNameAlreadyExistsException(name);

            Name = name;
        }
    }

    public void SetCoordinates(Coordinates? coordinates)
    {
        Coordinates = coordinates;
    }
}
