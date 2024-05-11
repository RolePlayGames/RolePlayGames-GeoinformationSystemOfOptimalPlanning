using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Application.Contracts.FilmTypes;

/// <summary>
/// Manages film type use cases
/// </summary>
public interface IFilmTypeSerivce
{
    /// <summary>
    /// Creates new film type
    /// </summary>
    /// <param name="filmType">Film type data</param>
    /// <returns>New film type ID</returns>
    Task<long> CreateFilmType(FilmTypeDTO filmType);

    /// <summary>
    /// Deletes film type
    /// </summary>
    /// <param name="id">Film type ID</param>
    Task DeleteFilmType(long id);

    /// <summary>
    /// Gets film type by ID
    /// </summary>
    /// <param name="id">Film type ID</param>
    /// <returns>Film type</returns>
    Task<FilmTypeDTO> GetFilmType(long id);

    /// <summary>
    /// Returns film types small information
    /// </summary>
    Task<IReadOnlyCollection<FilmTypeInfo>> GetFilmTypesInfo();

    /// <summary>
    /// Updates film type
    /// </summary>
    /// <param name="id">Film type ID</param>
    /// <param name="filmType">Film type data</param>
    Task UpdateFilmType(long id, FilmTypeDTO filmType);
}
