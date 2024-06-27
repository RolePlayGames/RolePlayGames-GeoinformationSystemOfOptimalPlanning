namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeDTO
{
    public required string Name { get; init; }

    public required long FilmTypeID { get; init; }

    public required double Thickness { get; init; }

    public required double ProductionSpeed { get; init; }

    public required double MaterialCost { get; init; }

    public required double Nozzle { get; init; }

    public required double Calibration { get; init; }

    public required double CoolingLip { get; init; }
}
