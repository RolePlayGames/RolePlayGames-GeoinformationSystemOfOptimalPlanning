namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineMaxProductionSpeed
{
    public const double Min = 0;

    private readonly double _maxProductionSpeed;

    public ProductionLineMaxProductionSpeed(double maxProductionSpeed)
    {
        if (maxProductionSpeed <= Min)
            throw new ArgumentOutOfRangeException(nameof(maxProductionSpeed), $"Max production speed should be greater than {Min}");

        _maxProductionSpeed = maxProductionSpeed;
    }

    public static implicit operator double(ProductionLineMaxProductionSpeed maxProductionSpeed) => maxProductionSpeed._maxProductionSpeed;

    public static explicit operator ProductionLineMaxProductionSpeed(double maxProductionSpeed) => new(maxProductionSpeed);
}
