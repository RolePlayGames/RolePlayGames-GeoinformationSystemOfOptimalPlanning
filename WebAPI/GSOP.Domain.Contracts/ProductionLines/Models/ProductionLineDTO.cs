using GSOP.Domain.Contracts.ProductionLines.ProductionRules;

namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineDTO
{
    public required string Name { get; init; }

    public required decimal HourCost { get; init; }

    public required double MaxProductionSpeed { get; init; }

    public required double WidthMin { get; init; }

    public required double WidthMax { get; init; }

    public required double ThicknessMin { get; init; }

    public required double ThicknessMax { get; init; }

    public required TimeSpan ThicknessChangeTime { get; init; }

    public required double ThicknessChangeConsumption { get; init; }

    public required TimeSpan WidthChangeTime { get; init; }

    public required double WidthChangeConsumption { get; init; }

    public required TimeSpan SetupTime { get; init; }

    public required IReadOnlyCollection<CalibratoinChangeRuleDTO> CalibratoinChangeRules { get; init; }

    public required IReadOnlyCollection<CoolingLipChangeRuleDTO> CoolingLipChangeRules { get; init; }

    public required IReadOnlyCollection<FilmTypeChangeRuleDTO> FilmTypeChangeRules { get; init; }

    public required IReadOnlyCollection<NozzleChangeRuleDTO> NozzleChangeRules { get; init; }

    public required long ProductionID { get; init; }
}
