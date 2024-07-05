using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.FilmRecipes;

/// <summary>
/// Manages film recipe database logic
/// </summary>
public interface IFilmRecipeRepository
{
    /// <summary>
    /// Creates film recipe in database
    /// </summary>
    /// <param name="filmRecipe">Film recipe</param>
    /// <returns>Generated id</returns>
    Task<long> Create(IFilmRecipe filmRecipe);

    /// <summary>
    /// Deletes film recipe
    /// </summary>
    /// <param name="id">Film recipe id</param>
    /// <returns>Is film recipe deleted</returns>
    Task<bool> Delete(ID id);

    /// <summary>
    /// Gets film recipe by id
    /// </summary>
    /// <param name="id">Film recipe id</param>
    /// <returns>Film recipe data or null</returns>
    Task<FilmRecipeDTO?> Get(ID id);

    /// <summary>
    /// Gets avaliable film types
    /// </summary>
    /// <returns>Avaliable film types</returns>
    Task<IReadOnlyCollection<AvaliableFilmType>> GetAvaliableFilmTypes();

    /// <summary>
    /// Gets film recipe short information
    /// </summary>
    /// <returns>Each film recipe info</returns>
    Task<IReadOnlyCollection<FilmRecipeInfo>> GetInfos();

    /// <summary>
    /// Is film type already exists
    /// </summary>
    /// <param name="id">>Film type id</param>
    /// <returns>True if film type exists</returns>
    Task<bool> IsFilmTypeExists(FilmTypeID id);

    /// <summary>
    /// Is film recipe name already exists
    /// </summary>
    /// <param name="filmRecipeName">Film recipe article</param>
    /// <returns>True if name exists</returns>
    Task<bool> IsNameExsits(FilmRecipeName filmRecipeName);

    /// <summary>
    /// Updates film recipe
    /// </summary>
    /// <param name="id">Film recipe id</param>
    /// <param name="filmRecipe">Film recipe</param>
    Task Update(ID id, IFilmRecipe filmRecipe);
}
