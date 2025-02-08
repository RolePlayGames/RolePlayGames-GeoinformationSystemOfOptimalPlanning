namespace GSOP.Domain.Contracts.ProductionLines.ProductionRules;

public record CoolingLipChangeRuleDTO
{
    public required double CoolingLipTo { get; init; }

    public required TimeSpan ChangeTime { get; init; }

    public required double ChangeConsumption { get; init; }
}
