namespace GSOP.Domain.Contracts.Locations;

public record CoordinatesDTO
{
    public required decimal Latitude { get; init; }

    public required decimal Longitude { get; init; }
}
