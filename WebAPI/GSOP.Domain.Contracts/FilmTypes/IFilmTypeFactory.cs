using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.Contracts.FilmTypes;

/// <summary>
/// Manages film type domain model creation
/// </summary>
public interface IFilmTypeFactory
{
    /// <summary>
    /// Creates film type by id from repository
    /// </summary>
    /// <param name="id">Film type id</param>
    /// <returns>Film type</returns>
    Task<IFilmType> CreateFilmType(long id);

    /// <summary>
    /// Creates and validates film type by data
    /// </summary>
    /// <param name="filmType">Film type data</param>
    /// <returns>Film type</returns>
    Task<IFilmType> CreateFilmType(FilmTypeDTO filmType);
}
