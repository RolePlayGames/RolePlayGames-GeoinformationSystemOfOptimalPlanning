using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.Contracts.FilmTypes;

/// <summary>
/// Film type domain model
/// </summary>
public interface IFilmType
{
    FilmTypeArticle Article { get; }

    /// <summary>
    /// Validates and updates film type article
    /// </summary>
    /// <param name="name">Film type article</param>
    Task SetArticle(FilmTypeArticle article);
}
