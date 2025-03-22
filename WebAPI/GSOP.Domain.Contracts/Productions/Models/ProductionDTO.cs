using GSOP.Domain.Contracts.Locations;

namespace GSOP.Domain.Contracts.Productions.Models;

public record ProductionDTO
{
    public required string Name { get; init; }

    public required CoordinatesDTO? Coordinates { get; init; }
}
