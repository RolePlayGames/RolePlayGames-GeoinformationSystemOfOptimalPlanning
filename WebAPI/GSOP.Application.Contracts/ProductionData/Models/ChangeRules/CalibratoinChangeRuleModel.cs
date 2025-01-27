namespace GSOP.Application.Contracts.ProductionData.Models.ChangeRules;

public record CalibratoinChangeRuleModel
{
    public required string ProductionLineName { get; init; }

    public required double CalibrationTo { get; init; }

    public required int ChangeTimeMinutes { get; init; }

    public required double ChangeConsumption { get; init; }
}
