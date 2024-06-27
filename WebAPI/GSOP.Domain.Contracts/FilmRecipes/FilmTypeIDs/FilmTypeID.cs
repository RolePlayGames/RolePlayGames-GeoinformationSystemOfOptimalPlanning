namespace GSOP.Domain.Contracts.FilmRecipes.FilmTypeIDs;

public record FilmTypeID
{
    public record IsFilmTypeExists
    {
        private readonly bool _isExists;

        public IsFilmTypeExists(bool isExists)
        {
            _isExists = isExists;
        }

        public static implicit operator bool(IsFilmTypeExists isExists) => isExists._isExists;

        public static explicit operator IsFilmTypeExists(bool isExists) => new(isExists);
    }

    private readonly ID _id;

    public FilmTypeID(ID id, IsFilmTypeExists isFilmTypeExists)
    {
        if (isFilmTypeExists)
            throw new ArgumentOutOfRangeException(nameof(isFilmTypeExists), "Film type is not exists");

        _id = id;
    }

    public static implicit operator long(FilmTypeID id) => id._id;

    public static async Task<IsFilmTypeExists> CheckIfFilmTypeExists(IFilmRecipeRepository repository, ID id)
    {
        var isFilmTypeExists = await repository.IsFilmTypeExists(id);

        return new IsFilmTypeExists(isFilmTypeExists);

    }
}
