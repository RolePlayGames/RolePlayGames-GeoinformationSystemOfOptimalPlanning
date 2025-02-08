namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public record NozzleChangeRuleDTO
{
    public required double NozzleTo { get; init; }

    public required TimeSpan ChangeTime { get; init; }

    public required double ChangeConsumption { get; init; }
}
