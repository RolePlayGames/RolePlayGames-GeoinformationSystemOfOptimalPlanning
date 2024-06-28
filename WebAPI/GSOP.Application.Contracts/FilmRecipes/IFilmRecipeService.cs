using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Application.Contracts.FilmRecipes;

/// <summary>
/// Manages film type use cases
/// </summary>
public interface IFilmRecipeService
{
    /// <summary>
    /// Creates new film recipe
    /// </summary>
    /// <param name="filmRecipe">Film recipe data</param>
    /// <returns>New film recipe ID</returns>
    Task<long> CreateFilmRecipe(FilmRecipeDTO filmRecipe);

    /// <summary>
    /// Deletes film recipe
    /// </summary>
    /// <param name="id">Film recipe ID</param>
    Task DeleteFilmRecipe(long id);

    /// <summary>
    /// Returns avaliable film types small information
    /// </summary>
    Task<IReadOnlyCollection<AvaliableFilmType>> GetAvaliableFilmTypes();

    /// <summary>
    /// Gets film recipe by ID
    /// </summary>
    /// <param name="id">Film recipe ID</param>
    /// <returns>Film recipe</returns>
    Task<FilmRecipeDTO> GetFilmRecipe(long id);

    /// <summary>
    /// Returns film recipes small information
    /// </summary>
    Task<IReadOnlyCollection<FilmRecipeInfo>> GetFilmRecipesInfo();

    /// <summary>
    /// Updates film recipe
    /// </summary>
    /// <param name="id">Film recipe ID</param>
    /// <param name="filmRecipe">Film recipe data</param>
    Task UpdateFilmRecipe(long id, FilmRecipeDTO filmRecipe);
}
