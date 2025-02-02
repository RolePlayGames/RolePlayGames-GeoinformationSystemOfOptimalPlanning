namespace GSOP.Application.Contracts.ProductionData.Models;

public record FilmRecipeModel
{
    public required string Name { get; init; }

    public required string FilmTypeArticle { get; init; }

    public required double Thickness { get; init; }

    public required double ProductionSpeed { get; init; }

    public required double MaterialCost { get; init; }

    public required double Nozzle { get; init; }

    public required double Calibration { get; init; }

    public required double CoolingLip { get; init; }
}
