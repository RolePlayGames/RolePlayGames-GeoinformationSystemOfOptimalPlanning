using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.Models;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.FilmRecipes;

/// <inheritdoc/>
public class FilmRecipeRepository : IFilmRecipeRepository
{
    private readonly DatabaseConnection _connection;

    public FilmRecipeRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task<long> Create(IFilmRecipe filmRecipe)
    {
        return _connection.InsertWithInt64IdentityAsync(new FilmRecipePOCO
        {
            Name = filmRecipe.Name,
            FilmTypeID = filmRecipe.FilmTypeID,
            Thickness = filmRecipe.Thickness,
            ProductionSpeed = filmRecipe.ProductionSpeed,
            MaterialCost = filmRecipe.MaterialCost,
            Nozzle = filmRecipe.Nozzle,
            Calibration = filmRecipe.Calibration,
            CoolingLip = filmRecipe.CoolingLip,
        });
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(ID id)
    {
        return await _connection.FilmRecipes
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<FilmRecipeDTO?> Get(ID id)
    {
        return _connection.FilmRecipes
            .Where(x => x.ID == id)
            .Select(x => new FilmRecipeDTO
            { 
                Name = x.Name,
                FilmTypeID = x.FilmTypeID,
                Thickness = x.Thickness,
                ProductionSpeed = x.ProductionSpeed,
                MaterialCost = x.MaterialCost,
                Nozzle = x.Nozzle,
                Calibration = x.Calibration,
                CoolingLip = x.CoolingLip,
            })
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<AvaliableFilmType>> GetAvaliableFilmTypes()
    {
        return await _connection.FilmTypes
            .Select(x => new AvaliableFilmType { ID = x.ID, Name = x.Article })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<FilmRecipeInfo>> GetInfos()
    {
        return await _connection.FilmRecipes
            .Select(x => new FilmRecipeInfo { ID = x.ID, Name = x.Name })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsFilmTypeExists(FilmTypeID id)
    {
        return _connection.FilmTypes
            .AnyAsync(x => x.ID == id);
    }

    /// <inheritdoc/>
    public Task<bool> IsNameExsits(FilmRecipeName filmRecipeName)
    {
        return _connection.FilmRecipes
            .AnyAsync(x => x.Name == filmRecipeName);
    }

    /// <inheritdoc/>
    public Task Update(ID id, IFilmRecipe filmRecipe)
    {
        return _connection.FilmRecipes
            .Where(x => x.ID == id)
            .Set(x => x.Name, filmRecipe.Name)
            .Set(x => x.FilmTypeID, filmRecipe.FilmTypeID)
            .Set(x => x.Thickness, filmRecipe.Thickness)
            .Set(x => x.ProductionSpeed, filmRecipe.ProductionSpeed)
            .Set(x => x.MaterialCost, filmRecipe.MaterialCost)
            .Set(x => x.Nozzle, filmRecipe.Nozzle)
            .Set(x => x.Calibration, filmRecipe.Calibration)
            .Set(x => x.CoolingLip, filmRecipe.CoolingLip)
            .UpdateAsync();
    }
}
