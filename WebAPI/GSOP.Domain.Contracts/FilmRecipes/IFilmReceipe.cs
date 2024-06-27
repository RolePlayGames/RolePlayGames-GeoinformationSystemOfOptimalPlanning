using GSOP.Domain.Contracts.FilmRecipes.FilmTypeIDs;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmTypes;

namespace GSOP.Domain.Contracts.FilmRecipes;

/// <summary>
/// Film recipe domain model
/// </summary>
public class IFilmRecipe
{
    public required FilmRecipeName Name { get; init; }

    public required FilmTypeID FilmTypeID { get; init; }

    public required IFilmType FilmType { get; init; }

    public required FilmRecipeThickness Thickness { get; init; }

    public required FilmRecipeProductionSpeed ProductionSpeed { get; init; }

    public required FilmRecipeMaterialCost MaterialCost { get; init; }

    public required FilmRecipeNozzle Nozzle { get; init; }

    public required FilmRecipeCalibration Calibration { get; init; }

    public required FilmRecipeCoolingLip CoolingLip { get; init; }
}
