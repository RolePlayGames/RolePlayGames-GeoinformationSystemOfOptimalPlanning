using GSOP.Domain.Contracts.FilmRecipes.Models;

namespace GSOP.Domain.Contracts.FilmRecipes.Exceptions;

/// <summary>
/// Represents film recipe's type does not exists
/// </summary>
public class FilmTypeDoesNotExistsException : Exception
{
    public long FilmTypeID { get; }

    public FilmTypeDoesNotExistsException(FilmTypeID filmTypeID) : base($"Film recipe's type {filmTypeID} should exists but does not")
    {
        FilmTypeID = filmTypeID;
    }
}
