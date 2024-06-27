using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmRecipes;
using GSOP.Domain.Contracts.FilmRecipes.FilmTypeIDs;

namespace GSOP.Domain.FilmRecipes.FilmTypeIDs;

/// <inheritdoc/>
public class FilmTypeIdFactory : IFilmTypeIdFactory
{
    private readonly IFilmRecipeRepository _repository;

    public FilmTypeIdFactory(IFilmRecipeRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public async Task<FilmTypeID> CreateFilmTypeID(long id)
    {
        var filmTypeID = new ID(id);
        var isFilmTypeExists = await FilmTypeID.CheckIfFilmTypeExists(_repository, filmTypeID);

        return new FilmTypeID(filmTypeID, isFilmTypeExists);
    }
}
