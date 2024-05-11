using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.Contracts.FilmTypes.Exceptions;

/// <summary>
/// Represents film type article is not unique
/// </summary>
public class FilmTypeArticleAlreadyExistsException : Exception
{
    public string FilmTypeArticle { get; }

    public FilmTypeArticleAlreadyExistsException(FilmTypeArticle filmTypeArticle) : base($"Film type article {filmTypeArticle} should be unique but already exists")
    {
        FilmTypeArticle = filmTypeArticle;
    }
}
