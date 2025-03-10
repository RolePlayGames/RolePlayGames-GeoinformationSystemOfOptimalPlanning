namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderWaste
{
    public const int Min = 0;

    private readonly double _orderWaste;

    public OrderWaste(double orderWaste)
    {
        if (orderWaste < Min)
            throw new ArgumentOutOfRangeException(nameof(orderWaste), $"Waste should be greater than or equal to {Min}");

        _orderWaste = orderWaste;
    }

    public static implicit operator double(OrderWaste rollsCount) => rollsCount._orderWaste;

    public static explicit operator OrderWaste(double rollsCount) => new(rollsCount);
}
