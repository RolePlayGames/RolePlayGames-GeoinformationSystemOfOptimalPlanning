using GSOP.Domain.Contracts.Optimization.Models;

namespace GSOP.Application.Contracts.Optimization.Models;

public record PlanningInfo
{
    public required IReadOnlyCollection<ProductionPlanInfo> ProductionPlans { get; init; }

    public required IReadOnlyCollection<RoutesQueueInfo> RoutesQueues { get; init; }
}
