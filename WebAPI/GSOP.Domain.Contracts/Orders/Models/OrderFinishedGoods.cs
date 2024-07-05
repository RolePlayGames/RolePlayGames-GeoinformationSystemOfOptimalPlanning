namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderFinishedGoods
{
    public const int Min = 0;

    private readonly int _finishedGoods;

    public OrderFinishedGoods(int finishedGoods)
    {
        if (finishedGoods <= Min)
            throw new ArgumentOutOfRangeException(nameof(finishedGoods), $"Finished goods should be greater than {Min}");

        _finishedGoods = finishedGoods;
    }

    public static implicit operator int(OrderFinishedGoods finishedGoods) => finishedGoods._finishedGoods;

    public static explicit operator OrderFinishedGoods(int finishedGoods) => new(finishedGoods);
}
