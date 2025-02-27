namespace GSOP.Domain.Contracts.Optimization.Models;

public record ProductionPlan
{
    public required IReadOnlyCollection<ProductionLineQueue> ProductionLineQueues { get; init; }
}
