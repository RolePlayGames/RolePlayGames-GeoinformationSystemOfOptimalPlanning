namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeNozzle
{
    public const int Min = 0;
    public const int Max = 300;

    private readonly double _nozzle;

    public FilmRecipeNozzle(double nozzle)
    {
        if (nozzle <= Min || nozzle > Max)
            throw new ArgumentOutOfRangeException(nameof(nozzle), $"Nozzle should be greater than {Min} and lesser than {Max}");

        _nozzle = nozzle;
    }

    public static implicit operator double(FilmRecipeNozzle nozzle) => nozzle._nozzle;

    public static explicit operator FilmRecipeNozzle(double nozzle) => new(nozzle);
}
