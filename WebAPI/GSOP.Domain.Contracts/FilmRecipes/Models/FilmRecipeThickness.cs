namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeThickness
{
    public const int Min = 0;
    public const int Max = 300;

    private readonly double _thickness;

    public FilmRecipeThickness(double thickness)
    {
        if (thickness <= Min || thickness > Max)
            throw new ArgumentOutOfRangeException(nameof(thickness), $"Thickness should be greater than {Min} and lesser than {Max}");

        _thickness = thickness;
    }

    public static implicit operator double(FilmRecipeThickness thickness) => thickness._thickness;

    public static explicit operator FilmRecipeThickness(double thickness) => new(thickness);
}
