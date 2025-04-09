namespace GSOP.Domain.Contracts.Routes.Models;

public record RouteCreateDTO
{
    public required long ProductionID { get; init; }

    public required long CustomerID { get; init; }

    public required double Price { get; init; }

    public required TimeSpan DrivingTime { get; init; }
}
