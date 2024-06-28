using GSOP.Application.Contracts.FilmRecipes;
using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Exceptions;
using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Application.FilmRecipes;

/// <inheritdoc/>
public class FilmRecipeService : IFilmRecipeService
{
    private readonly IFilmRecipeFactory _filmRecipeFactory;
    private readonly IFilmRecipeRepository _filmRecipeRepository;

    public FilmRecipeService(IFilmRecipeFactory filmRecipeFactory, IFilmRecipeRepository filmRecipeRepository)
    {
        _filmRecipeFactory = filmRecipeFactory;
        _filmRecipeRepository = filmRecipeRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateFilmRecipe(FilmRecipeDTO filmRecipe)
    {
        var newFilmType = await _filmRecipeFactory.Create(filmRecipe);

        return await _filmRecipeRepository.Create(newFilmType);
    }

    /// <inheritdoc/>
    public async Task DeleteFilmRecipe(long id)
    {
        var filmRecipeId = new ID(id);

        var isfilmRecipeDeleted = await _filmRecipeRepository.Delete(filmRecipeId);

        if (!isfilmRecipeDeleted)
            throw new FilmRecipeWasNotFoundException(filmRecipeId);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<AvaliableFilmType>> GetAvaliableFilmTypes()
    {
        return _filmRecipeRepository.GetAvaliableFilmTypes();
    }

    /// <inheritdoc/>
    public async Task<FilmRecipeDTO> GetFilmRecipe(long id)
    {
        var filmRecipeId = new ID(id);
        return await _filmRecipeRepository.Get(filmRecipeId) ?? throw new FilmRecipeWasNotFoundException(filmRecipeId);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<FilmRecipeInfo>> GetFilmRecipesInfo()
    {
        return _filmRecipeRepository.GetInfos();
    }

    /// <inheritdoc/>
    public async Task UpdateFilmRecipe(long id, FilmRecipeDTO filmRecipe)
    {
        var filmRecipeId = new ID(id);
        var filmRecipeName = new FilmRecipeName(filmRecipe.Name);
        var filmTypeID = new FilmTypeID(filmRecipe.FilmTypeID);
        var thickness = new FilmRecipeThickness(filmRecipe.Thickness);
        var productionSpeed = new FilmRecipeProductionSpeed(filmRecipe.ProductionSpeed);
        var materialCost = new FilmRecipeMaterialCost(filmRecipe.MaterialCost);
        var nozzle = new FilmRecipeNozzle(filmRecipe.Nozzle);
        var calibration = new FilmRecipeCalibration(filmRecipe.Calibration);
        var coolingLip = new FilmRecipeCoolingLip(filmRecipe.CoolingLip);

        var existingFilmRecipe = await _filmRecipeFactory.Create(id);

        await existingFilmRecipe.SetName(filmRecipeName);
        await existingFilmRecipe.SetFilmTypeID(filmTypeID);
        existingFilmRecipe.SetThickness(thickness);
        existingFilmRecipe.SetProductionSpeed(productionSpeed);
        existingFilmRecipe.SetMaterialCost(materialCost);
        existingFilmRecipe.SetNozzle(nozzle);
        existingFilmRecipe.SetCalibration(calibration);
        existingFilmRecipe.SetCoolingLip(coolingLip);

        await _filmRecipeRepository.Update(filmRecipeId, existingFilmRecipe);
    }
}
