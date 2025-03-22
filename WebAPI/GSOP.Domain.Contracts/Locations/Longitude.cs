namespace GSOP.Domain.Contracts.Locations;

public record Longitude
{
    private readonly decimal _longitude;

    public Longitude(decimal longitude)
    {
        _longitude = longitude;
    }

    public static implicit operator decimal(Longitude longitude) => longitude._longitude;

    public static explicit operator Longitude(decimal longitude) => new(longitude);

    public static implicit operator decimal?(Longitude? longitude) => longitude?._longitude;

    public static explicit operator Longitude?(decimal? longitude) => longitude.HasValue ? new(longitude.Value) : null;
}
