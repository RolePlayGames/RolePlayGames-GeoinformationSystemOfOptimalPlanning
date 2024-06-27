namespace GSOP.Domain.Contracts.FilmRecipes.FilmTypeIDs;

public class FilmTypeIdFactory : IFilmTypeIdFactory
{
    private readonly IFilmRecipeRepository _repository;

    public FilmTypeIdFactory(IFilmRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<FilmTypeID> CreateFilmTypeID(long id)
    {
        var filmTypeID = new ID(id);
        var isFilmTypeExists = await FilmTypeID.CheckIfFilmTypeExists(_repository, filmTypeID);

        return new FilmTypeID(filmTypeID, isFilmTypeExists);
    }
}
