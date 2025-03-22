using GSOP.Domain.Contracts.Locations;
using GSOP.Domain.Contracts.Productions.Models;

namespace GSOP.Domain.Contracts.Productions;

public interface IProduction
{
    ProductionName Name { get; }

    Coordinates? Coordinates { get; }

    Task SetName(ProductionName name);

    void SetCoordinates(Coordinates? coordinates);
}
