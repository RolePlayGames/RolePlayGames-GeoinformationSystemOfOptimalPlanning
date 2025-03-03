namespace GSOP.Domain.Contracts.FilmRecipes.Models;

public record FilmRecipeCalibration : IComparable<FilmRecipeCalibration>
{
    public const int Min = 0;
    public const int Max = 600;

    private readonly double _calibration;

    public FilmRecipeCalibration(double calibration)
    {
        if (calibration <= Min || calibration > Max)
            throw new ArgumentOutOfRangeException(nameof(calibration), $"Calibration should be greater than {Min} and lesser than {Max}");

        _calibration = calibration;
    }

    public int CompareTo(FilmRecipeCalibration? other)
    {
        if (other is null)
            return -1;

        return _calibration.CompareTo(other._calibration);
    }

    public static implicit operator double(FilmRecipeCalibration calibration) => calibration._calibration;

    public static explicit operator FilmRecipeCalibration(double calibration) => new(calibration);
}
