using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Application.Contracts.Optimization.Models;

public record RoutesQueueInfo
{
    public required RouteReadDTO Route { get; init; }

    public required IReadOnlyCollection<OrderRouteInfo> OrderPosition { get; init; }
}
