namespace GSOP.Domain.Contracts.Orders.Models;

public record OrderWidth
{
    public const int Min = 0;
    public const int Max = 10000;

    private readonly int _width;

    public OrderWidth(int width)
    {
        if (width <= Min || width > Max)
            throw new ArgumentOutOfRangeException(nameof(width), $"Width should be greater than {Min} and lower than {Max}");

        _width = width;
    }

    public static implicit operator int(OrderWidth width) => width._width;

    public static explicit operator OrderWidth(int width) => new(width);
}
