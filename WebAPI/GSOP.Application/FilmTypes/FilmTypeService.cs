using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Domain.Contracts.FilmTypes.Exceptions;
using GSOP.Application.Contracts.FilmTypes;

namespace GSOP.Application.FilmTypes;

/// <inheritdoc/>
public class FilmTypeService : IFilmTypeSerivce
{
    private readonly IFilmTypeFactory _filmTypeFactory;
    private readonly IFilmTypeRepository _filmTypeRepository;

    public FilmTypeService(IFilmTypeFactory filmTypeFactory, IFilmTypeRepository filmTypeRepository)
    {
        _filmTypeFactory = filmTypeFactory;
        _filmTypeRepository = filmTypeRepository;
    }

    /// <inheritdoc/>
    public async Task<long> CreateFilmType(FilmTypeDTO filmType)
    {
        var newFilmType = await _filmTypeFactory.CreateFilmType(filmType);

        return await _filmTypeRepository.Create(newFilmType);
    }

    /// <inheritdoc/>
    public async Task DeleteFilmType(long id)
    {
        var filmTypeId = new ID(id);

        var isfilmTypeDeleted = await _filmTypeRepository.Delete(filmTypeId);

        if (!isfilmTypeDeleted)
            throw new FilmTypeWasNotFoundException(filmTypeId);
    }

    /// <inheritdoc/>
    public async Task<FilmTypeDTO> GetFilmType(long id)
    {
        var filmTypeId = new ID(id);
        var filmType = await _filmTypeRepository.Get(filmTypeId);

        if (filmType is null)
            throw new FilmTypeWasNotFoundException(filmTypeId);

        return filmType;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<FilmTypeInfo>> GetFilmTypesInfo()
    {
        return _filmTypeRepository.GetInfos();
    }

    /// <inheritdoc/>
    public async Task UpdateFilmType(long id, FilmTypeDTO filmType)
    {
        var filmTypeId = new ID(id);
        var filmTypeArticle = new FilmTypeArticle(filmType.Article);

        var existingFilmType = await _filmTypeFactory.CreateFilmType(id);

        await existingFilmType.SetArticle(filmTypeArticle);

        await _filmTypeRepository.Update(filmTypeId, existingFilmType);
    }
}
