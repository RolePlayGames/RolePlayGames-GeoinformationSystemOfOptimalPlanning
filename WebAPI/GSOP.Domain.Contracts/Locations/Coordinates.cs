namespace GSOP.Domain.Contracts.Locations;

public record Coordinates
{
    public Latitude Latitude { get; }

    public Longitude Longitude { get; }

    public Coordinates(Latitude latitude, Longitude longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
