namespace GSOP.Application.Contracts.Optimization.Models;

public record ProductionLineQueueInfo
{
    public required string ProductionLineName { get; init; }

    public required IReadOnlyCollection<OrderPositionInfo> OrderPositions { get; init; }
}
