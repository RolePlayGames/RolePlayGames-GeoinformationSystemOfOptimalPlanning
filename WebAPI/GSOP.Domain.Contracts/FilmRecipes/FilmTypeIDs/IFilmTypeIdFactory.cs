namespace GSOP.Domain.Contracts.FilmRecipes.FilmTypeIDs;

/// <summary>
/// Manages film type id for film recipe creation
/// </summary>
public interface IFilmTypeIdFactory
{
    /// <summary>
    /// Verifies and creates film type id
    /// </summary>
    /// <param name="id">Film type id</param>
    /// <returns>Verified film type id</returns>
    Task<FilmTypeID> CreateFilmTypeID(long id);
}
