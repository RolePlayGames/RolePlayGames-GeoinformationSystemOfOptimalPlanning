using GSOP.Domain.Contracts.Locations;

namespace GSOP.Domain.Contracts.Routes.Models;

public record EntityLocationInfo
{
    public required long EntityID { get; init; }

    public required string EntityName { get; init; }

    public required CoordinatesDTO? EntityCoordinates { get; init; }
}
