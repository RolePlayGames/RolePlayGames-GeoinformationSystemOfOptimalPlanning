namespace GSOP.Domain.Contracts.Routes.Models;

public record RoutePrice
{
    public const int Min = 0;

    private readonly double _price;

    public RoutePrice(double price)
    {
        if (price < Min)
            throw new ArgumentOutOfRangeException(nameof(price), $"Route price should be greater than or equal to {Min}");

        _price = price;
    }

    public static implicit operator double(RoutePrice routePrice) => routePrice._price;

    public static explicit operator RoutePrice(double routePrice) => new(routePrice);
}
