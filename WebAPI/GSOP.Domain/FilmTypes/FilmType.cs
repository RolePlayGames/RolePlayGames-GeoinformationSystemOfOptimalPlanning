using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;
using GSOP.Domain.Contracts.FilmTypes.Models;

namespace GSOP.Domain.FilmTypes;

public class FilmType : IFilmType
{
    private readonly IFilmTypeRepository _filmTypeRepository;

    public FilmTypeArticle Article { get; protected set; }

    public FilmType(FilmTypeArticle article, IFilmTypeRepository filmTypeRepository)
    {
        Article = article;
        _filmTypeRepository = filmTypeRepository;
    }

    /// <inheritdoc/>
    public async Task SetArticle(FilmTypeArticle article)
    {
        if (Article != article)
        {
            var isArticleExists = await _filmTypeRepository.IsArticleExsits(article);

            if (isArticleExists)
                throw new FilmTypeArticleAlreadyExistsException(article);

            Article = article;
        }
    }
}
