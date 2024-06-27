namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeProductionSpeed
{
    public const int Min = 0;
    public const int Max = 300;

    private readonly double _productionSpeed;

    public FilmRecipeProductionSpeed(double productionSpeed)
    {
        if (productionSpeed <= Min || productionSpeed > Max)
            throw new ArgumentOutOfRangeException(nameof(productionSpeed), $"Production speed should be greater than {Min} and lesser than {Max}");

        _productionSpeed = productionSpeed;
    }

    public static implicit operator double(FilmRecipeProductionSpeed productionSpeed) => productionSpeed._productionSpeed;

    public static explicit operator FilmRecipeProductionSpeed(double productionSpeed) => new(productionSpeed);
}
