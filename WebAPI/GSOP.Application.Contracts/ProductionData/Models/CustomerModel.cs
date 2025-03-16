namespace GSOP.Application.Contracts.ProductionData.Models;

public record CustomerModel
{
    public required string Name { get; init; }

    public required decimal? Latitude { get; init; }

    public required decimal? Longitude { get; init; }
}
