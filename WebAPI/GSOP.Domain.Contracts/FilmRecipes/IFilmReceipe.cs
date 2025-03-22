using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.FilmRecipes;

/// <summary>
/// Film recipe domain model
/// </summary>
public interface IFilmRecipe
{
    ID ID { get; }

    FilmRecipeName Name { get; }

    FilmTypeID FilmTypeID { get; }

    FilmRecipeThickness Thickness { get; }

    FilmRecipeProductionSpeed ProductionSpeed { get; }

    FilmRecipeMaterialCost MaterialCost { get; }

    FilmRecipeNozzle Nozzle { get; }

    FilmRecipeCalibration Calibration { get; }

    FilmRecipeCoolingLip CoolingLip { get; }

    Task SetName(FilmRecipeName name);

    Task SetFilmTypeID(FilmTypeID filmTypeId);

    void SetThickness(FilmRecipeThickness thickness);

    void SetProductionSpeed(FilmRecipeProductionSpeed productionSpeed);

    void SetMaterialCost(FilmRecipeMaterialCost materialCost);

    void SetNozzle(FilmRecipeNozzle nozzle);

    void SetCalibration(FilmRecipeCalibration calibration);

    void SetCoolingLip(FilmRecipeCoolingLip coolingLip);
}
