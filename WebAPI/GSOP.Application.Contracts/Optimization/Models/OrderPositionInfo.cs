namespace GSOP.Application.Contracts.Optimization.Models;

public record OrderPositionInfo
{
    public required string OrderNumber { get; init; }

    public required DateTime OrderProductionStartDateTime { get; init; }

    public required TimeSpan OrderProductionDuration { get; init; }
}
