namespace GSOP.Application.Contracts.ProductionData.Models.ChangeRules;

public record NozzleChangeRuleModel
{
    public required string ProductionLineName { get; init; }

    public required double NozzleTo { get; init; }

    public required int ChangeTimeMinutes { get; init; }

    public required double ChangeConsumption { get; init; }
}
