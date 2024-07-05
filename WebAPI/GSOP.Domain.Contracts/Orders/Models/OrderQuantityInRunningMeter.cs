namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderQuantityInRunningMeter
{
    public const int Min = 0;

    private readonly int _quantityInRunningMeter;

    public OrderQuantityInRunningMeter(int quantityInRunningMeter)
    {
        if (quantityInRunningMeter <= Min)
            throw new ArgumentOutOfRangeException(nameof(quantityInRunningMeter), $"Quantity in running meter should be greater than {Min}");

        _quantityInRunningMeter = quantityInRunningMeter;
    }

    public static implicit operator int(OrderQuantityInRunningMeter quantityInRunningMeter) => quantityInRunningMeter._quantityInRunningMeter;

    public static explicit operator OrderQuantityInRunningMeter(int quantityInRunningMeter) => new(quantityInRunningMeter);
}
