namespace GSOP.Application.Contracts.ProductionData.Models;

public record ProductionLineModel
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
}
