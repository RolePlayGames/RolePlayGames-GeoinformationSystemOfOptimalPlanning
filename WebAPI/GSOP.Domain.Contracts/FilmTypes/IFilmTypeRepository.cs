using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.Contracts.FilmTypes;

/// <summary>
/// Manages film type database logic
/// </summary>
public interface IFilmTypeRepository
{
    /// <summary>
    /// Creates film type in database
    /// </summary>
    /// <param name="filmType">Film type</param>
    /// <returns>Generated id</returns>
    Task<long> Create(IFilmType filmType);

    /// <summary>
    /// Deletes film type
    /// </summary>
    /// <param name="id">Film type id</param>
    /// <returns>Is film type deleted</returns>
    Task<bool> Delete(ID id);

    /// <summary>
    /// Gets film type by id
    /// </summary>
    /// <param name="id">Film type id</param>
    /// <returns>Film type data or null</returns>
    Task<FilmTypeDTO?> Get(ID id);

    /// <summary>
    /// Gets film type short information
    /// </summary>
    /// <returns>Each film type info</returns>
    Task<IReadOnlyCollection<FilmTypeInfo>> GetInfos();

    /// <summary>
    /// Is film type name already exists
    /// </summary>
    /// <param name="filmTypeArticle">Film type article</param>
    /// <returns>True if article is exists</returns>
    Task<bool> IsArticleExsits(FilmTypeArticle filmTypeArticle);

    /// <summary>
    /// Updates film type
    /// </summary>
    /// <param name="id">Film type id</param>
    /// <param name="filmType">Film type</param>
    Task Update(ID id, IFilmType filmType);
}
