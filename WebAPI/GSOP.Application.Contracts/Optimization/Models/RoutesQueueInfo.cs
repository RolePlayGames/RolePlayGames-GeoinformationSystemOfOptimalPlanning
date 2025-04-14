using GSOP.Domain.Contracts.Customers.Models;
using GSOP.Domain.Contracts.Routes.Models;

namespace GSOP.Application.Contracts.Optimization.Models;

public record RoutesQueueInfo
{
    public required EntityLocationInfo ProductionInfo { get; init; }

    public required IReadOnlyCollection<EntityLocationInfo> CustomerInfos { get; init; }
}
