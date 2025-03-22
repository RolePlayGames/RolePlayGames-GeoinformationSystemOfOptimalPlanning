namespace GSOP.Domain.Contracts.Locations;

public record Latitude
{
    private readonly decimal _latitude;

    public Latitude(decimal latitude)
    {
        _latitude = latitude;
    }

    public static implicit operator decimal(Latitude latitude) => latitude._latitude;

    public static explicit operator Latitude(decimal latitude) => new(latitude);

    public static implicit operator decimal?(Latitude? latitude) => latitude?._latitude;

    public static explicit operator Latitude?(decimal? latitude) => latitude.HasValue ? new(latitude.Value) : null;
}
