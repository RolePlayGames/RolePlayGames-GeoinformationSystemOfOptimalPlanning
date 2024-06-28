using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.FilmRecipes.Exceptions;

/// <summary>
/// Represents film recipe name is not unique
/// </summary>
public class FilmRecipeNameAlreadyExistsException : Exception
{
    public string FilmRecipeName { get; }

    public FilmRecipeNameAlreadyExistsException(FilmRecipeName filmRecipeName) : base($"Film recipe name {filmRecipeName} should be unique but already exists")
    {
        FilmRecipeName = filmRecipeName;
    }
}
