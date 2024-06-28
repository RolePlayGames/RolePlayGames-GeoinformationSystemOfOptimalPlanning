using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.FilmRecipes;

/// <summary>
/// Manages film recipe domain model creation
/// </summary>
public interface IFilmRecipeFactory
{
    /// <summary>
    /// Creates film recipe by id from repository
    /// </summary>
    /// <param name="id">Film recipe id</param>
    /// <returns>Film recipe</returns>
    Task<IFilmRecipe> Create(long id);

    /// <summary>
    /// Creates and validates film recipe by data
    /// </summary>
    /// <param name="filmRecipe">Film recipe data</param>
    /// <returns>Film recipe</returns>
    Task<IFilmRecipe> Create(FilmRecipeDTO filmRecipe);
}
