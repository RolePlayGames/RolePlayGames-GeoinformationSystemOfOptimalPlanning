namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeNozzle : IComparable<FilmRecipeNozzle>
{
    public const int Min = 0;

    private readonly double _nozzle;

    public FilmRecipeNozzle(double nozzle)
    {
        if (nozzle <= Min)
            throw new ArgumentOutOfRangeException(nameof(nozzle), $"Nozzle should be greater than {Min} ");

        _nozzle = nozzle;
    }

    public int CompareTo(FilmRecipeNozzle? other)
    {
        if (other is null)
            return -1;

        return _nozzle.CompareTo(other._nozzle);
    }

    public static implicit operator double(FilmRecipeNozzle nozzle) => nozzle._nozzle;

    public static explicit operator FilmRecipeNozzle(double nozzle) => new(nozzle);
}
