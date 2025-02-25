using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;
using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.FilmRecipes;

/// <inheritdoc/>
public class FilmRecipeFactory : IFilmRecipeFactory
{
    private readonly IFilmRecipeRepository _filmRecipeRepository;

    public FilmRecipeFactory(IFilmRecipeRepository filmRecipeRepository)
    {
        _filmRecipeRepository = filmRecipeRepository;
    }

    /// <inheritdoc/>
    public async Task<IFilmRecipe> Create(long id)
    {
        var filmRecipeID = new ID(id);

        var filmRecipe = await _filmRecipeRepository.Get(filmRecipeID) ?? throw new FilmRecipeWasNotFoundException(filmRecipeID);

        var name = new FilmRecipeName(filmRecipe.Name);
        var filmTypeID = new FilmTypeID(filmRecipe.FilmTypeID);
        var thickness = new FilmRecipeThickness(filmRecipe.Thickness);
        var productionSpeed = new FilmRecipeProductionSpeed(filmRecipe.ProductionSpeed);
        var materialCost = new FilmRecipeMaterialCost(filmRecipe.MaterialCost);
        var nozzle = new FilmRecipeNozzle(filmRecipe.Nozzle);
        var calibration = new FilmRecipeCalibration(filmRecipe.Calibration);
        var coolingLip = new FilmRecipeCoolingLip(filmRecipe.CoolingLip);

        return new FilmRecipe(
            filmRecipeID,
            name,
            filmTypeID,
            thickness,
            productionSpeed,
            materialCost,
            nozzle,
            calibration,
            coolingLip,
            _filmRecipeRepository);
    }

    /// <inheritdoc/>
    public async Task<IFilmRecipe> Create(FilmRecipeDTO filmRecipe)
    {
        var name = new FilmRecipeName(filmRecipe.Name);

        var isFilmRecipeNameExsits = await _filmRecipeRepository.IsNameExsits(name);

        if (isFilmRecipeNameExsits)
            throw new FilmRecipeNameAlreadyExistsException(name);

        var filmTypeID = new FilmTypeID(filmRecipe.FilmTypeID);

        var isFilmTypeExists = await _filmRecipeRepository.IsFilmTypeExists(filmTypeID);

        if (!isFilmTypeExists)
            throw new FilmTypeDoesNotExistsException(filmTypeID);

        var thickness = new FilmRecipeThickness(filmRecipe.Thickness);
        var productionSpeed = new FilmRecipeProductionSpeed(filmRecipe.ProductionSpeed);
        var materialCost = new FilmRecipeMaterialCost(filmRecipe.MaterialCost);
        var nozzle = new FilmRecipeNozzle(filmRecipe.Nozzle);
        var calibration = new FilmRecipeCalibration(filmRecipe.Calibration);
        var coolingLip = new FilmRecipeCoolingLip(filmRecipe.CoolingLip);

        return new FilmRecipe(
            new ID(0),
            name,
            filmTypeID,
            thickness,
            productionSpeed,
            materialCost,
            nozzle,
            calibration,
            coolingLip,
            _filmRecipeRepository);
    }
}
