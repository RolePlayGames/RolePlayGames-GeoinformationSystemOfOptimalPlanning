using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;

namespace GSOP.Domain.FilmTypes;

/// <inheritdoc/>
public class FilmTypeFactory : IFilmTypeFactory
{
    private readonly IFilmTypeRepository _filmTypeRepository;

    public FilmTypeFactory(IFilmTypeRepository filmTypeRepository)
    {
        _filmTypeRepository = filmTypeRepository;
    }

    /// <inheritdoc/>
    public async Task<IFilmType> CreateFilmType(long id)
    {
        var filmTypeId = new ID(id);

        var filmType = await _filmTypeRepository.Get(filmTypeId);

        if (filmType is null)
            throw new FilmTypeWasNotFoundException(filmTypeId);

        var name = new FilmTypeArticle(filmType.Article);

        return new FilmType(name, _filmTypeRepository);
    }

    /// <inheritdoc/>
    public async Task<IFilmType> CreateFilmType(FilmTypeDTO filmType)
    {
        var article = new FilmTypeArticle(filmType.Article);

        var isCustomerNameExsits = await _filmTypeRepository.IsArticleExsits(article);

        if (isCustomerNameExsits)
            throw new FilmTypeArticleAlreadyExistsException(article);

        return new FilmType(article, _filmTypeRepository);
    }
}
