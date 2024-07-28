namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderPriceOverdue
{
    public const int Min = 0;

    private readonly double _priceOverdue;

    public OrderPriceOverdue(double priceOverdue)
    {
        if (priceOverdue < Min)
            throw new ArgumentOutOfRangeException(nameof(priceOverdue), $"Price overdue should be greater than {Min}");

        _priceOverdue = priceOverdue;
    }

    public static implicit operator double(OrderPriceOverdue priceOverdue) => priceOverdue._priceOverdue;

    public static explicit operator OrderPriceOverdue(double priceOverdue) => new(priceOverdue);
}
