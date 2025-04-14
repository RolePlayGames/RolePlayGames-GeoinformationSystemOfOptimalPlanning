using GSOP.Application.Contracts.Optimization.Models;

namespace GSOP.Domain.Contracts.Optimization.Models;

public record ProductionPlanInfo
{
    public required DateTime StartDateTime { get; init; }

    public required IReadOnlyCollection<ProductionLineQueueInfo> ProductionLineQueues { get; init; }

    public required double TargetFunctionValue { get; init; }
}
