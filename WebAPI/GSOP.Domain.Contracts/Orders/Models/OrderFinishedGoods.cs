namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderFinishedGoods
{
    public const int Min = 0;

    private readonly double _finishedGoods;

    public OrderFinishedGoods(double finishedGoods)
    {
        if (finishedGoods < Min)
            throw new ArgumentOutOfRangeException(nameof(finishedGoods), $"Finished goods should be greater than or equal to {Min}");

        _finishedGoods = finishedGoods;
    }

    public static implicit operator double(OrderFinishedGoods finishedGoods) => finishedGoods._finishedGoods;

    public static explicit operator OrderFinishedGoods(double finishedGoods) => new(finishedGoods);
}
