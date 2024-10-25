using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.Contracts.FilmTypes;

/// <summary>
/// Film type domain model
/// </summary>
public interface IFilmType
{
    FilmTypeArticle Article { get; }

    Task SetArticle(FilmTypeArticle article);
}
