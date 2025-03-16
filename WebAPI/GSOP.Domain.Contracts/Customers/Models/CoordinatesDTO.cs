namespace GSOP.Domain.Contracts.Customers.Models;

public record CoordinatesDTO
{
    public required decimal Latitude { get; init; }

    public required decimal Longitude { get; init; }
}
