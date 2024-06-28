namespace GSOP.Domain.Contracts.FilmRecipes.Exceptions;

/// <summary>
/// Represents film recipe was not found by ID
/// </summary>
public class FilmRecipeWasNotFoundException : Exception
{
    public long ID { get; }

    public FilmRecipeWasNotFoundException(ID id) : base($"Film recipe was not found by ID {id}")
    {
        ID = id;
    }
}
