namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderRollsCount
{
    public const int Min = 0;

    private readonly int _rollsCount;

    public OrderRollsCount(int rollsCount)
    {
        if (rollsCount <= Min)
            throw new ArgumentOutOfRangeException(nameof(rollsCount), $"Rolls count should be greater than {Min}");

        _rollsCount = rollsCount;
    }

    public static implicit operator int(OrderRollsCount rollsCount) => rollsCount._rollsCount;

    public static explicit operator OrderRollsCount(int rollsCount) => new(rollsCount);
}
