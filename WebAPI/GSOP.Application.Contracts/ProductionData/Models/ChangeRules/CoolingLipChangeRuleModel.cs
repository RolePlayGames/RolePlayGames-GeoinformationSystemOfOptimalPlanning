namespace GSOP.Application.Contracts.ProductionData.Models.ChangeRules;

public record CoolingLipChangeRuleModel
{
    public required string ProductionLineName { get; init; }

    public required double CoolingLipTo { get; init; }

    public required int ChangeTimeMinutes { get; init; }

    public required double ChangeConsumption { get; init; }
}
