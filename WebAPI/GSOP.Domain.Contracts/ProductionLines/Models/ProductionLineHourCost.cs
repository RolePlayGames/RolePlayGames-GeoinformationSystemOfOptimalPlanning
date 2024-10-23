namespace GSOP.Domain.Contracts.ProductionLines.Models;

public record ProductionLineHourCost
{
    public const decimal Min = 0;

    private readonly decimal _hourCost;

    public ProductionLineHourCost(decimal hourCost)
    {
        if (hourCost <= Min)
            throw new ArgumentOutOfRangeException(nameof(hourCost), $"Hour cost count should be greater than {Min}");

        _hourCost = hourCost;
    }

    public static implicit operator decimal(ProductionLineHourCost hourCost) => hourCost._hourCost;

    public static explicit operator ProductionLineHourCost(decimal hourCost) => new(hourCost);
}
