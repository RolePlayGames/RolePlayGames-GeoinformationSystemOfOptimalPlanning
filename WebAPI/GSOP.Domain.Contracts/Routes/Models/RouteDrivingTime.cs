namespace GSOP.Domain.Contracts.Routes.Models;

public record RouteDrivingTime
{
    public readonly TimeSpan Min = TimeSpan.Zero;

    private readonly TimeSpan _time;

    public RouteDrivingTime(TimeSpan time)
    {
        if (time < Min)
            throw new ArgumentOutOfRangeException(nameof(time), $"Route driving time should be greater than or eqaul to {Min}");

        _time = time;
    }

    public static implicit operator TimeSpan(RouteDrivingTime priceOverdue) => priceOverdue._time;

    public static explicit operator RouteDrivingTime(TimeSpan priceOverdue) => new(priceOverdue);
}
