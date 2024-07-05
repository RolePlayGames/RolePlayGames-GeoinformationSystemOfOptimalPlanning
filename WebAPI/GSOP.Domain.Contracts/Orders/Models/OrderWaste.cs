namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderWaste
{
    public const int Min = 0;

    private readonly int _orderWaste;

    public OrderWaste(int orderWaste)
    {
        if (orderWaste <= Min)
            throw new ArgumentOutOfRangeException(nameof(orderWaste), $"Waste should be greater than {Min}");

        _orderWaste = orderWaste;
    }

    public static implicit operator int(OrderWaste rollsCount) => rollsCount._orderWaste;

    public static explicit operator OrderWaste(int rollsCount) => new(rollsCount);
}
