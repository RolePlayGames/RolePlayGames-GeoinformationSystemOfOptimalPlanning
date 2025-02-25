using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;
using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.FilmRecipes;

/// <inheritdoc/>
public record FilmRecipe : IFilmRecipe
{
    private readonly IFilmRecipeRepository _filmRecipeRepository;

    public ID ID { get; }

    public FilmRecipeName Name { get; protected set; }

    public FilmTypeID FilmTypeID { get; protected set; }

    public FilmRecipeThickness Thickness { get; protected set; }

    public FilmRecipeProductionSpeed ProductionSpeed { get; protected set; }

    public FilmRecipeMaterialCost MaterialCost { get; protected set; }

    public FilmRecipeNozzle Nozzle { get; protected set; }

    public FilmRecipeCalibration Calibration { get; protected set; }

    public FilmRecipeCoolingLip CoolingLip { get; protected set; }

    public FilmRecipe(
        ID id,
        FilmRecipeName name,
        FilmTypeID filmTypeID,
        FilmRecipeThickness thickness,
        FilmRecipeProductionSpeed productionSpeed,
        FilmRecipeMaterialCost materialCost,
        FilmRecipeNozzle nozzle,
        FilmRecipeCalibration calibration,
        FilmRecipeCoolingLip coolingLip,
        IFilmRecipeRepository filmRecipeRepository)
    {
        ID = id;
        Name = name;
        FilmTypeID = filmTypeID;
        Thickness = thickness;
        ProductionSpeed = productionSpeed;
        MaterialCost = materialCost;
        Nozzle = nozzle;
        Calibration = calibration;
        CoolingLip = coolingLip;
        _filmRecipeRepository = filmRecipeRepository;
    }

    /// <inheritdoc/>
    public async Task SetName(FilmRecipeName name)
    {
        if (Name != name)
        {
            var isNameExists = await _filmRecipeRepository.IsNameExsits(name);

            if (isNameExists)
                throw new FilmRecipeNameAlreadyExistsException(name);

            Name = name;
        }
    }

    /// <inheritdoc/>
    public async Task SetFilmTypeID(FilmTypeID filmTypeId)
    {
        if (FilmTypeID != filmTypeId)
        {
            var isFilmTypeExists = await _filmRecipeRepository.IsFilmTypeExists(filmTypeId);

            if (!isFilmTypeExists)
                throw new FilmTypeDoesNotExistsException(filmTypeId);

            FilmTypeID = filmTypeId;
        }
    }

    /// <inheritdoc/>
    public void SetThickness(FilmRecipeThickness thickness)
    {
        Thickness = thickness;
    }

    /// <inheritdoc/>
    public void SetProductionSpeed(FilmRecipeProductionSpeed productionSpeed)
    {
        ProductionSpeed = productionSpeed;
    }

    /// <inheritdoc/>
    public void SetMaterialCost(FilmRecipeMaterialCost materialCost)
    {
        MaterialCost = materialCost;
    }

    /// <inheritdoc/>
    public void SetNozzle(FilmRecipeNozzle nozzle)
    {
        Nozzle = nozzle;
    }

    /// <inheritdoc/>
    public void SetCalibration(FilmRecipeCalibration calibration)
    {
        Calibration = calibration;
    }

    /// <inheritdoc/>
    public void SetCoolingLip(FilmRecipeCoolingLip coolingLip)
    {
        CoolingLip = coolingLip;
    }
}
