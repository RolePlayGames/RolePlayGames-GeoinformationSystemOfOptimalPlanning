namespace GSOP.Domain.Contracts.FilmRecipes.FilmTypeIDs;

public interface IFilmTypeIdFactory
{
    Task<FilmTypeID> CreateFilmTypeID(long id);
}
