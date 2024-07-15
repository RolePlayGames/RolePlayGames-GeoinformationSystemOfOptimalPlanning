using GSOP.Domain.Contracts.Orders.Models;

namespace GSOP.Domain.Contracts.Orders.Exceptions;

/// <summary>
/// Represents order's film type does not exists
/// </summary>
public class FilmRecipeDoesNotExistsException : Exception
{
    public long FilmRecipeID { get; }

    public FilmRecipeDoesNotExistsException(FilmRecipeID filmRecipeID) : base($"Order's film recipe {filmRecipeID} should exists but does not")
    {
        FilmRecipeID = filmRecipeID;
    }
}
