using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.FilmRecipes;

/// <summary>
/// Film recipe domain model
/// </summary>
public interface IFilmRecipe
{
    FilmRecipeName Name { get; }

    FilmTypeID FilmTypeID { get; }

    FilmRecipeThickness Thickness { get; }

    FilmRecipeProductionSpeed ProductionSpeed { get; }

    FilmRecipeMaterialCost MaterialCost { get; }

    FilmRecipeNozzle Nozzle { get; }

    FilmRecipeCalibration Calibration { get; }

    FilmRecipeCoolingLip CoolingLip { get; }

    /// <summary>
    /// Validates and updates film type article (should be unique)
    /// </summary>
    /// <param name="name">Film type article</param>
    Task SetName(FilmRecipeName name);

    /// <summary>
    /// Validates and updates film type id (should exists)
    /// </summary>
    /// <param name="typeID">Film type ID</param>
    Task SetFilmTypeID(FilmTypeID filmTypeID);

    /// <summary>
    /// Updates thickness
    /// </summary>
    /// <param name="thickness">Thickness</param>
    void SetThickness(FilmRecipeThickness thickness);

    /// <summary>
    /// Updates production speed
    /// </summary>
    /// <param name="productionSpeed">Production speed</param>
    void SetProductionSpeed(FilmRecipeProductionSpeed productionSpeed);

    /// <summary>
    /// Updates material cost
    /// </summary>
    /// <param name="materialCost">Material cost</param>
    void SetMaterialCost(FilmRecipeMaterialCost materialCost);

    /// <summary>
    /// Updates nozzle
    /// </summary>
    /// <param name="nozzle">Nozzle</param>
    void SetNozzle(FilmRecipeNozzle nozzle);

    /// <summary>
    /// Updates calibration
    /// </summary>
    /// <param name="calibration">Calibration</param>
    void SetCalibration(FilmRecipeCalibration calibration);

    /// <summary>
    /// Updates coolingLip
    /// </summary>
    /// <param name="coolingLip">CoolingLip</param>
    void SetCoolingLip(FilmRecipeCoolingLip coolingLip);
}
