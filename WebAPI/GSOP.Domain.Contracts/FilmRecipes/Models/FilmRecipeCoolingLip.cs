namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeCoolingLip
{
    public const int Min = 0;
    public const int Max = 1000;

    private readonly double _coolingLip;

    public FilmRecipeCoolingLip(double coolingLip)
    {
        if (coolingLip <= Min || coolingLip > Max)
            throw new ArgumentOutOfRangeException(nameof(coolingLip), $"Cooling lip should be greater than {Min} and lesser than {Max}");

        _coolingLip = coolingLip;
    }

    public static implicit operator double(FilmRecipeCoolingLip coolingLip) => coolingLip._coolingLip;

    public static explicit operator FilmRecipeCoolingLip(double coolingLip) => new(coolingLip);
}
